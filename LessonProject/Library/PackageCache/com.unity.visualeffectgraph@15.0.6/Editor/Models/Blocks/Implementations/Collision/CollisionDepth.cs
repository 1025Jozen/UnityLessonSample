using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityEditor.VFX.Block
{
    [VFXHelpURL("Block-CollideWithDepthBuffer")]
    [VFXInfo(category = "Collision")]
    class CollisionDepth : CollisionBase
    {
        enum SurfaceThickness
        {
            Infinite,
            Custom,
        }

        [VFXSetting, Tooltip("Specifies which Camera to use for the particles to collide with its depth buffer. Can use the camera tagged 'Main', or a custom camera.")]
        public CameraMode camera;

        [VFXSetting, SerializeField, Tooltip("Specifies the thickness mode for the colliding surface. It can have an infinite thickness, or be set to a custom value.")]
        SurfaceThickness surfaceThickness = SurfaceThickness.Infinite;

        public override string name { get { return "Collide with Depth Buffer"; } }

        public class ThicknessProperties
        {
            [Min(0.0f)]
            public float surfaceThickness = 1.0f;
        }

        protected override bool allowInvertedCollision { get { return false; } }

        internal sealed override void GenerateErrors(VFXInvalidateErrorReporter manager)
        {
            base.GenerateErrors(manager);
            if (camera == CameraMode.Main && (UnityEngine.Rendering.RenderPipelineManager.currentPipeline == null || !UnityEngine.Rendering.RenderPipelineManager.currentPipeline.ToString().Contains("HDRenderPipeline")))
                manager.RegisterError("CollisionDepthUnavailableWithoutHDRP", VFXErrorType.Warning, "Depth collision is currently only supported in the High Definition Render Pipeline (HDRP).");
        }

        public override IEnumerable<VFXAttributeInfo> attributes
        {
            get
            {
                foreach (var a in base.attributes)
                    yield return a;
            }
        }

        protected override IEnumerable<VFXPropertyWithValue> inputProperties
        {
            get
            {
                var inputs = base.inputProperties;
                if (camera == CameraMode.Custom)
                    inputs = inputs.Concat(PropertiesFromType(typeof(CameraHelper.CameraProperties)));
                if (surfaceThickness == SurfaceThickness.Custom)
                    inputs = inputs.Concat(PropertiesFromType("ThicknessProperties"));
                return inputs;
            }
        }

        public override IEnumerable<VFXNamedExpression> parameters
        {
            get
            {
                var expressions = CameraHelper.AddCameraExpressions(base.parameters, camera);

                VFXSpace systemSpace = ((VFXDataParticle)GetData()).space;
                // in custom camera mode, camera space is already in system space (conversion happened in slot)
                CameraMatricesExpressions camMat = CameraHelper.GetMatricesExpressions(expressions, camera == CameraMode.Main ? VFXSpace.World : systemSpace, systemSpace);

                // Filter unused expressions
                expressions = expressions.Where(t =>
                    t.name != "Camera_fieldOfView" &&
                    t.name != "Camera_aspectRatio" &&
                    t.name != "Camera_transform" &&
                    t.name != "Camera_colorBuffer" &&
                    t.name != "Camera_orthographicSize");

                foreach (var e in expressions)
                    yield return e;

                yield return camMat.ViewToVFX;
                yield return camMat.VFXToView;
                yield return camMat.ViewToClip;
                yield return camMat.ClipToView;
            }
        }

        public override string source
        {
            get
            {
                string Source = @"
float3 nextPos = position + velocity * deltaTime;
float3 viewPos = mul(VFXToView,float4(nextPos,1.0f)).xyz;

float4 projPos = mul(ViewToClip,float4(viewPos,1.0f));
projPos.xyz /= projPos.w;
float2 aProjPos = abs(projPos.xy);

if (aProjPos.x < 1.0f && aProjPos.y < 1.0f) // visible on screen
{
    float2 uv = projPos.xy * 0.5f + 0.5f;
    float depth = LOAD_TEXTURE2D_X(Camera_depthBuffer.t, uv*Camera_pixelDimensions).r;
    #if UNITY_REVERSED_Z
    depth = 1.0f - depth; // reversed z
    #endif

    const float n = Camera_nearPlane;
    const float f = Camera_farPlane;
    float linearEyeDepth, offset;
    if (!Camera_orthographic)
    {
        linearEyeDepth = n * f / (depth * (n - f) + f);
        offset = 2.0f;
    }
    else
    {
        linearEyeDepth = n + depth * (f-n);
        offset = 32.0f; //Orthographic depth requires a larger offset to give out correct normals
    }";

                if (surfaceThickness == SurfaceThickness.Infinite)
                    Source += @"
    if (viewPos.z > linearEyeDepth - radius)";
                else
                    Source += @"
    if (viewPos.z > linearEyeDepth - radius && viewPos.z < linearEyeDepth + radius + surfaceThickness)";

                Source += @"
    {
        const float2 pixelOffset = offset / Camera_pixelDimensions;

        float2 projPos10 = projPos.xy + float2(pixelOffset.x,0.0f);
        float2 projPos01 = projPos.xy + float2(0.0f,pixelOffset.y);

        int2 depthPos10 = clamp(int2((projPos10 * 0.5f + 0.5f) * Camera_pixelDimensions), 0, Camera_pixelDimensions - 1);
        int2 depthPos01 = clamp(int2((projPos01 * 0.5f + 0.5f) * Camera_pixelDimensions), 0, Camera_pixelDimensions - 1);

        float depth10 = LOAD_TEXTURE2D_X(Camera_depthBuffer.t, depthPos10).r;
        float depth01 = LOAD_TEXTURE2D_X(Camera_depthBuffer.t, depthPos01).r;

        #if UNITY_REVERSED_Z
        depth10 = 1.0f - depth10;
        depth01 = 1.0f - depth01;
        #endif

        float4 vPos10 = mul(ClipToView,float4(projPos10,depth10 * 2.0f - 1.0f,1.0f));
        float4 vPos01 = mul(ClipToView,float4(projPos01,depth01 * 2.0f - 1.0f,1.0f));

        vPos10.xyz /= vPos10.w;
        vPos01.xyz /= vPos01.w;
        float r = 0.0f;
        if(!Camera_orthographic)
        {
            r = linearEyeDepth / viewPos.z;
        }
        else
        {
            // Will work for planar surface, not curved, because the normal nEstimate is evaluated away from the real projection point
            float3 pSurface = viewPos;
            pSurface.z = linearEyeDepth;
            float3 nEstimate = normalize(cross(vPos01.xyz - pSurface,vPos10.xyz - pSurface));
            r = dot(pSurface,nEstimate)/dot(viewPos,nEstimate);
        }
        viewPos *= r; // Position on depth surface

        float3 n = normalize(cross(vPos01.xyz - viewPos,vPos10.xyz - viewPos));
        n = normalize(mul((float3x3)ViewToVFX,n));

        viewPos *= 1.0f - radius / linearEyeDepth; // Push based on radius
        position = mul(ViewToVFX,float4(viewPos,1.0f)).xyz;
";

                Source += collisionResponseSource;
                Source += @"
    }
}";

                return Source;
            }
        }
    }
}
