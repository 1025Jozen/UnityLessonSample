using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    //マウスでクリックしたところのピクセルを黒くする
    public class PixAccess1 : MonoBehaviour
    {
        Texture2D drawTexture;
        Color[] buffer;

        void Start() {
            Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
            Color[] pixels = mainTexture.GetPixels();

            buffer = new Color[pixels.Length];
            pixels.CopyTo(buffer, 0);

            //お絵描き用のテクスチャ
            drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
            drawTexture.filterMode = FilterMode.Point;
        }



        void Update() {
            if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f)) {
                    Draw(hit.textureCoord * 256);//レイが当たった箇所のUV座標(0~1)を取得できる
                }

                drawTexture.SetPixels(buffer);//テクスチャに　ピクセル情報をセットする
                drawTexture.Apply();          //確定させる
                GetComponent<MeshRenderer>().material.mainTexture = drawTexture;//テクスチャをMeshRenderにセットしたマテリアルに割り当てる
            }
        }

        //(マウスが乗っている)1ピクセルだけを書き換え
        public void Draw(Vector2 p) {
            buffer.SetValue(Color.black, (int)p.x + 256 * (int)p.y);//ピクセル　色合いに関する　配列　を　該当座標にセットする
        }
    }
}