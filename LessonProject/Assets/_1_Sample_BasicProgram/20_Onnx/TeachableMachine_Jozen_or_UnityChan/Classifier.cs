using System;
using Unity.Barracuda;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// 分類
public class Classifier : MonoBehaviour
{
    // リソース
    public NNModel modelFile;    // モデル
    public TextAsset labelsFile; // ラベル

    // パラメータ
    public const int IMAGE_SIZE   = 224;    // 画像サイズ
    private const int IMAGE_MEAN  = 127;    // MEAN
    private const float IMAGE_STD = 127.5f; // STD
    private const string INPUT_NAME = "sequential_1_input"; //★ onnxファイルのインスペクターから入力名を各自変更
    private const string OUTPUT_NAME = "sequential_3";       //★ onnxファイルのインスペクターから出力名を各自変更


    //private const string INPUT_NAME = "input"; //★ onnxファイルのインスペクターから入力名を各自変更
    //private const string OUTPUT_NAME = "MobilenetV2/Predictions/Reshape_1";       //★ onnxファイルのインスペクターから出力名を各自変更

    // 推論
    private IWorker worker;  // ワーカー
    private string[] labels; // ラベル
    private int waitIndex = 0;

    void Start() {
        this.labels = Regex.Split(this.labelsFile.text, "\n|\r|\r\n")//ラベルの読み込み
            .Where(s => !String.IsNullOrEmpty(s)).ToArray();
        var model = ModelLoader.Load(this.modelFile);//モデルの読み込み
        Debug.Log(labels[0]);
        Debug.Log(labels[1]);
        
        this.worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);// ワーカー(エンジン)の生成
    }

    // 推論の実行
    public IEnumerator Predict(Color32[] picture, System.Action<List<KeyValuePair<string, float>>> callback) {        
        var map = new List<KeyValuePair<string, float>>();// 結果

        // 入力テンソルの生成
        using (var tensor = TransformInput(picture, IMAGE_SIZE, IMAGE_SIZE)) {            
            var inputs = new Dictionary<string, Tensor>();// 入力の生成
            inputs.Add(INPUT_NAME, tensor);         
            var enumerator = this.worker.StartManualSchedule(inputs);// 推論の実行

            // 推論の実行の完了待ち
            while (enumerator.MoveNext()) {
                waitIndex++;
                if (waitIndex >= 20) {
                    waitIndex = 0;
                    yield return null;
                }
            };
   
            var output = worker.PeekOutput(OUTPUT_NAME);// 出力の生成
            for (int i = 0; i < labels.Length; i++) {
                map.Add(new KeyValuePair<string, float>(labels[i], output[i] * 100));
            }
        } 
        callback(map.OrderByDescending(x => x.Value).ToList());// ソートして結果を返す
    }

    // 入力テンソルの生成
    public static Tensor TransformInput(Color32[] pic, int width, int height) {
        float[] floatValues = new float[width * height * 3];
        for (int i = 0; i < pic.Length; ++i) {
            var color = pic[i];
            floatValues[i * 3 + 0] = (color.r - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 1] = (color.g - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 2] = (color.b - IMAGE_MEAN) / IMAGE_STD;
        }
        return new Tensor(1, height, width, 3, floatValues);
    }

    private void OnDestroy() {
        worker?.Dispose();// 使い終わったらお片付け
    }
}
