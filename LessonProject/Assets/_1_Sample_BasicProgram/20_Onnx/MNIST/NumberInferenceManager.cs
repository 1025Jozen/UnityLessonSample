using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//(1) プロジェクトに.onnxファイル(モデルファイル)を追加
//(2) モデルのロード
//(3) 推論エンジン（Worker）の生成
//(4) モデルの実行
//(5) 結果の取得
namespace MNIST
{
    public class NumberInferenceManager : MonoBehaviour
    {
        private const int TexturePixelSize = 28;

        [SerializeField] private NNModel model;
        [SerializeField] private RawImage image;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private MnistInferencer _mnistInferencer;//MNIST推論

        private void Start() {
            _mnistInferencer = new MnistInferencer(model);//MNIST推論に関するスクリプトのインスタンス化
        }

        private void OnDestroy() {
            _mnistInferencer.Dispose();// 使い終わったらお片付け
        }

        private void Update() {
            var colors = (image.texture as Texture2D).GetPixels();     //ピクセルをRawImageから取ってくる　Colorの配列
            var pixels = new float[TexturePixelSize, TexturePixelSize];

            for (var i = 0; i < TexturePixelSize * TexturePixelSize; i++) {
                pixels[i % TexturePixelSize, i / TexturePixelSize] = colors[i].grayscale;
            }

            var result      = _mnistInferencer.Execute(pixels);//MNIST推論モデルからエンジンを作り配列を読み込ませて、結果を得る
            numberText.text = result.Number.ToString(); //結果からなんの数字と判定したか
            scoreText.text  = result.Score.ToString();  //どれくらいの確率か
        }
    }
}