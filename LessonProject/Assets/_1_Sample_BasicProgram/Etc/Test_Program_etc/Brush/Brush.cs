using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shader_Sample
{
    public class Brush : MonoBehaviour
    {
        public int brushWidth  = 150;
        public int brushHeight = 150;
        public Color color = Color.blue;

        public Color[] colors { get; set; }

        private Color colorOutSide = Color.black;   //color の下に付け足す

        public void UpdateBrushColor() {
            colors = new Color[brushWidth * brushHeight];//塗るサイズ分の配列
            for (int i = 0; i < colors.Length; i++) {
                colors[i] = color;//色を入れる
            }
            //Vector2 center = new Vector2(brushWidth / 2, brushHeight / 2);
            //colors = new Color[brushWidth * brushHeight];

            //for (int i = 0; i < colors.Length; i++) {
            //    float x = i % brushWidth;
            //    float y = Mathf.Floor(i / brushWidth);
            //    Vector2 pixelPos = new Vector2(x, y);
            //    float dist = Vector2.Distance(pixelPos, center);

            //    if (dist < brushWidth / 2) {
            //        colors[i] = color;
            //    } else {
            //        colors[i] = colorOutSide;
            //    }
            //}
        }

    }
}