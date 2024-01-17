using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    public class UV_Painter : MonoBehaviour
    {
        [SerializeField] Brush _brush;        //ブラシ
        [SerializeField] GameObject _paintObj;
        Texture2D _tex;


        void Start() {

            //ペイントをする対象のオブジェクトにセットしてあるテクスチャと同じサイズのテクスチャ２Dを作ります。
            _tex = new Texture2D(
                _paintObj.GetComponent<MeshRenderer>().material.mainTexture.width,
                _paintObj.GetComponent<MeshRenderer>().material.mainTexture.height
                );

            //Graphics.CopyTexture()で元のテクスチャをコピーして先ほど作成したテクスチャ２Dに入れています。
            //Graphics.CopyTexture(コピー元のテクスチャ、コピー元テクスチャの要素、コピー元テクスチャのミップマップレベル、コピー先テクスチャ、コピー元テクスチャの要素、コピー先テクスチャのミップマップレベル)
            Graphics.CopyTexture(_paintObj.GetComponent<MeshRenderer>().material.mainTexture, 0, 0, _tex, 0, 0);
            _brush.UpdateBrushColor();
        }

        void Update() {
            if (!Input.GetMouseButton(0)) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Renderer renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                MeshCollider meshCollider = hit.collider as MeshCollider;

                if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null) {
                    Debug.Log("NULL");
                    return;
                }


                //ヒットした所のUV座標を取得
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= _tex.width;
                pixelUV.y *= _tex.height;
                // Debug.Log("pixelUV:::" + (int)pixelUV.x + " , " + (int)pixelUV.y);

                // hit.textureCoordでは値が０から１で帰ってくるので、
                // それをテクスチャのサイズ分だけ乗算してピクセルの座標の値に変換しています。
                //位置をずらしてテクスチャの真ん中とタップ位置を合わせる
                _tex.SetPixels((int)pixelUV.x - _brush.brushWidth / 2, (int)pixelUV.y - _brush.brushHeight / 2, _brush.brushWidth, _brush.brushHeight, _brush.colors);
                _tex.Apply();
                renderer.material.mainTexture = _tex;
            }
        }
    }
}