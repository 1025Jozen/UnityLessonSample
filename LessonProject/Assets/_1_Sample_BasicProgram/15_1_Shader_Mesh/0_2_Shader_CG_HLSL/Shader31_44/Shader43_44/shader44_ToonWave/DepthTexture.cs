using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    [ExecuteAlways]
    public class DepthTexture : MonoBehaviour
    {
        private Camera cam;

        void Start() {
            cam = GetComponent<Camera>();
            cam.depthTextureMode = DepthTextureMode.Depth;
        }
    }
}

