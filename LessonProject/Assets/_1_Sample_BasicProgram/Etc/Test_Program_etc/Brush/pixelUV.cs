using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    public class pixelUV : MonoBehaviour
    {
        void Update() {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Vector2 pixelUV = hit.textureCoord;//オブジェクトのUV値を取得
                Debug.Log("pixelUV:::" + pixelUV.x + " , " + pixelUV.y);
            }
        }
    }
}

