using System;
using System.Linq;
using Unity.Barracuda;

//(1) プロジェクトに.onnxファイル(モデルファイル)を追加
//(2) モデルのロード
//(3) 推論エンジン（Worker）の生成
//(4) モデルの実行
//(5) 結果の取得
namespace MNIST
{
    public class MnistInferencer : IDisposable//参照の破棄 interface
    {
        private readonly IWorker _worker;

        public MnistInferencer(NNModel model) {
            var runtimeModel = ModelLoader.Load(model);        //モデルを読み込む
            _worker = WorkerFactory.CreateWorker(runtimeModel);//Worker(推論してくれるエンジン)を作成する
        }

        /// <summary>推論をする</summary>
        /// <returns>一番確率の大きい数字</returns>
        public Result Execute(float[,] input) {
            //784ピクセル
            // Mnistは28x28のグレースケールな画像、画像を1x28x28x1のTensor(とりあえず「特殊な行列」とでも思って)に変換する
            using var inputTensor = new Tensor(n: 1, h: 28, w: 28, c: 1);

            for (var y = 0; y < 28; y++) {
                for (var x = 0; x < 28; x++) {
                    inputTensor[0, 27 - y, x, 0] = input[x, y];//画像をテンソルに変換
                }
            }


            _worker.Execute(inputTensor);     //テンソルから　 推論を実行する
            var output = _worker.PeekOutput();//推論から結果を得る　// 出力を取得する


            // 0 ~ 1の確率に
            var scores = Enumerable.Range(0, 10)
                .Select(i => output[0, 0, 0, i])//outputから結果を丸める
                .SoftMax()
                .ToArray();

            // 確率が一番大きい数字を探す
            var max = 0f;
            var maxIndex = 0;
            for (var i = 0; i < scores.Length; i++) {//float配列の中をみて　スコアが一番高いものをセット
                if (scores[i] > max) {
                    max = scores[i];//推論が何％にあたるか
                    maxIndex = i;   //何番目の要素か(0~9のどの数字にあたるか)
                }
            }

            return new Result(maxIndex, max);//推論結果を返す
        }

        public void Dispose() {
            _worker?.Dispose();// 使い終わったらお片付け
        }

        /// <summary>推論結果</summary>
        public readonly struct Result
        {
            public int Number { get; }
            public float Score { get; }

            public Result(int number, float score) {
                Number = number;
                Score = score;
            }
        }
    }
}