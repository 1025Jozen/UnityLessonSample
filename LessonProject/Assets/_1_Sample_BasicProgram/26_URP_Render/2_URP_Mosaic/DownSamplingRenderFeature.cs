using UnityEngine.Rendering.Universal;

//手順②URP拡張の最小単位「Feature」の実装
//Featureの2つの役割

//①Passの生成
//②生成したPassをScriptableRendererに渡すこと   描画処理を記述したScriptableRenderPassをScriptableRendererにわたすことで初めて描画処理が走ります。  FeatureはScriptableRendererとの橋渡しを担っている
public class DownSamplingRenderFeature : ScriptableRendererFeature
{
    public int downSample = 10;
    
    DownSamplingRenderPass _renderPass;//渡すPass

    //①Passの生成
    public override void Create()
    {
        _renderPass = new DownSamplingRenderPass();//渡すPassの生成　インスタンス変数に保持
    }


    //②生成したPassをScriptableRendererに渡す
    /// ここでは、レンダラーに1つまたは複数のレンダーパスを注入することができます。
    /// このメソッドは、レンダラーをカメラごとに設定する際に呼び出されます。
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスにカメラのカラーを渡す
        renderer.EnqueuePass(_renderPass);// ScriptableRendererにPassを渡す   // Featureの中でPassを生成し、ScriptableRendererにPassを渡します。今回パスは1つしか扱いませんが、 Featureは複数のパスを管理することができます。
    }

    //「描画前のPassに対してFeatureからパラメータを渡す」
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {
        _renderPass.SetParam(renderer.cameraColorTargetHandle, downSample);
        //Passsで設定したメソッド実行
        //今回はPass側に 「SetParamメソッド」を定義して、描画前にFeatureからパラメータを渡すように実装しています。

        //具体的に描画前のパスに渡しているのは次の2つ「カメラ情報」と「ダウンサンプル値」
        //・カメラ情報　　　　カメラが写している画像情報 です。カメラの解像度はExecuteメソッドの引数RenderingDataから取得可能
        //・ダウンサンプル値　モザイク表現するための値。この値を毎フレーム変化させることでモザイクアニメーションを表現
    }


}


