using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    public class PixAccess2 : MonoBehaviour
    {
        Texture2D drawTexture;
        Color[] buffer;

        void Start() {
            Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
            Color[] pixels = mainTexture.GetPixels();

            buffer = new Color[pixels.Length];
            pixels.CopyTo(buffer, 0);

            drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
            drawTexture.filterMode = FilterMode.Point;
        }


        

        void Update() {
            if (Input.GetMouseButton(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f)) {
                    Draw(hit.textureCoord * 256);//レイが当たった箇所のUV座標を取得できる
                }

                drawTexture.SetPixels(buffer);//テクスチャに　ピクセル情報をセットする
                drawTexture.Apply();          //テクスチャを確定させる
                GetComponent<MeshRenderer>().material.mainTexture = drawTexture;//テクスチャをMeshRenderにセットしたマテリアルに割り当てる
            }
        }


        //ピクセル　色合いに関する　配列　を　該当座標にセットする
        //テクスチャ上のすべてのピクセルをチェックし、
        //マウスが乗っている座標からの距離が5以下なら黒をセットする
        public void Draw(Vector2 p) {
            for (int x = 0; x < 256; x++) {
                for (int y = 0; y < 256; y++) {
                    if ((p - new Vector2(x, y)).magnitude < 5) {
                        buffer.SetValue(Color.black, x + 256 * y);//第２引数 変更する要素の位置　　　buffer配列の中身　Color[ y 1列目 256個の要素　,  y 2列目 256個の要素 ,  ・・・]
                    }
                }
            }
        }

    }
}