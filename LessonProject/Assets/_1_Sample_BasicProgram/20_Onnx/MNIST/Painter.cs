using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MNIST
{
    public class Painter : MonoBehaviour
    {
        private const int TexturePixelSize = 28;

        [SerializeField] private RawImage image;     //描く場所
        [SerializeField] private Button deleteButton;

        private Texture2D _texture;       
        private PointerEventData _pointer;

        private void Start() {
            //横幅　高さのピクセル数などの設定
            _texture = new Texture2D(TexturePixelSize, TexturePixelSize,
                　　　　　　　　　　　　　TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point//テクスチャがそのままサンプリングされ、ＭｉｐＭａｐ境界がパッキリ割れる
            };

            image.texture = _texture;//上記のテクスチャを割り当てる

            //入力情報
            _pointer = new PointerEventData(EventSystem.current);//入力情報をとってくるクラスのインスタンス化

            ResetColors();//初期は全ピクセルを真っ黒に
            deleteButton.onClick.AddListener(ResetColors);//ボタンに「テクスチャを黒くし直す」メソッドをセット
        }

        private void OnDestroy() {
            if (_texture == null) return;
            Destroy(_texture);//テクスチャの破棄
            _texture = null;
        }

        private void Update() {            
            if (Input.GetMouseButton(0)) {//レイ情報からマウスをクリックしている間、描くという処理をする
                var results = new List<RaycastResult>();
                _pointer.position = Input.mousePosition;//マウスの位置
                EventSystem.current.RaycastAll(_pointer, results);

                foreach (var target in results.Where(target => target.gameObject == image.gameObject)) {
                    Draw(target);
                }
            }
        }

        //レイから画面での座標を
        private void Draw(RaycastResult target) {
            var corners = new Vector3[4];//(UI要素rawimageの座標４隅のための配列)
            image.rectTransform.GetWorldCorners(corners);//UI要素の四隅の座標の配列をrawimageにセットする
            corners[0] = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);//ワールド座標をスクリーン座標に変換
            corners[2] = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[2]);

            var width  = corners[2].x - corners[0].x;
            var height = corners[2].y - corners[0].y;

            var x = (int)((target.screenPosition.x - corners[0].x) / width * TexturePixelSize);
            var y = (int)((target.screenPosition.y - corners[0].y) / height * TexturePixelSize);

            //_texture.SetPixel(x, y, Color.white);
            for (var i = -1; i <= 1; i++) {
                for (var j = -1; j <= 1; j++) {
                    if (x + i < 0 || x + i >= TexturePixelSize || y + j < 0 || y + j >= TexturePixelSize) continue;//範囲外なら　forの先頭に戻る
                    _texture.SetPixel(x + i, y + j, Color.white);//テクスチャの該当ピクセルを白い色に
                }
            }

            _texture.Apply();//テクスチャを適用
        }

        private void ResetColors() {
            var colors = new Color[TexturePixelSize * TexturePixelSize];
            colors = colors.Select(color => Color.black).ToArray();

            _texture.SetPixels(colors);
            _texture.Apply();
        }
    }
}