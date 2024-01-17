using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shader_Sample
{
    public class SnowNoise : MonoBehaviour
    {
        [SerializeField] Shader _snowFallShader;//時間が経つにつれて_Displacementを
        private Material _snowFallMat;
        private MeshRenderer _meshRenderer;

        [Range(0.001f,0.1f)]
        [SerializeField] float _flakeAmount;
        [Range(0f, 1f)]
        [SerializeField] float _flakeOpacity;

        void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _snowFallMat = new Material(_snowFallShader);
        }

        void Update() {

            _snowFallMat.SetFloat("_FlakeAmount", _flakeAmount);
            _snowFallMat.SetFloat("_FlakeOpacity", _flakeOpacity);
            RenderTexture snow = (RenderTexture)_meshRenderer.material.GetTexture("_DispTex");
            RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(snow,temp, _snowFallMat);
            Graphics.Blit(temp,snow);
            _meshRenderer.material.SetTexture("_DispTex", snow);
            RenderTexture.ReleaseTemporary(temp);
            
        }
    }
}