using UnityEngine;
using System.Collections;


//テクスチャから　ピクセル数を把握し　ピクセルの色を指定して　　下半分だけを黒くする　　例　
namespace Shader_Sample
{
    public class PixAccess0 : MonoBehaviour
    {

        Texture2D drawTexture;
        Color[] buffer;

        void Start() {
            Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;//MeshRendererにアタッチしたマテリアルに設定したテクスチャを取得
            Color[] pixels = mainTexture.GetPixels(); //テクスチャのピクセル

            buffer = new Color[pixels.Length];//ピクセルの数だけの配列を用意
            pixels.CopyTo(buffer, 0);//buffer配列にpixels配列の中身をコピーする

            // 画面上半分を塗りつぶす
            for (int x = 0; x < mainTexture.width; x++) {
                for (int y = 0; y < mainTexture.height; y++) {
                    if (y < mainTexture.height / 2) {
                        buffer.SetValue(Color.black, x + 256 * y);//ピクセルごとの色合い情報を保持
                    }
                }
            }

            //テクスチャの生成
            drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
            drawTexture.filterMode = FilterMode.Point;//ピクセルを 1 つ 1 つブロックのように表示する
        }

        void Update() {
            drawTexture.SetPixels(buffer);//テクスチャに適応
            drawTexture.Apply();　　　　　　//確定
            GetComponent<Renderer>().material.mainTexture = drawTexture;//テクスチャの割り当て
        }
    }
}