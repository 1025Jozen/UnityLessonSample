using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.XR.XRDisplaySubsystem;


//手順①Passの実装
//最初にPass（パス）の実装をしていきます。Passには2つの役割があります。
//1つは 「描画処理の実装」 。
//もう1つはその 「描画処理の実行タイミングの定義」 です。
public class DownSamplingRenderPass : ScriptableRenderPass
{
    private const string CommandBufferName = nameof(DownSamplingRenderPass);
    private const int RenderTextureId = 0;
    
    private RenderTargetIdentifier _currentTarget;
    
    private int _downSample = 1;


    // ①描画処理を記述  
    /// ここでは、レンダリングロジックを実装することができます。
    /// 描画コマンドの発行やコマンドバッファの実行には、<c>ScriptableRenderContext</c>を使用します。
    /// https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    /// ScriptableRenderContext.submitを呼び出す必要はありません。
    /// レンダリングパイプラインがパイプライン内の特定のポイントで呼び出します。
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var commandBuffer = CommandBufferPool.Get(CommandBufferName);// CommandBufferPool からCommandBufferNameを取り出す。　CommandBuffer作成（描画コマンド） 
        var cameraData = renderingData.cameraData;//カメラデータをとってくる
        // 現在描画しているカメラの解像度を　「_downSample」で除算 
        var w = cameraData.camera.scaledPixelWidth / _downSample;
        var h = cameraData.camera.scaledPixelHeight / _downSample;
         
        commandBuffer.GetTemporaryRT(RenderTextureId, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);// 小さいサイズのRenderTextureを新規生成 
        commandBuffer.Blit(_currentTarget, RenderTextureId);// 現在のカメラ描画画像を小さいサイズのRenderTextureにコピー       
        commandBuffer.Blit(RenderTextureId, _currentTarget);// 小さいサイズのRenderTextureを現在のRenderTarget（カメラ）にコピー(RenderTarget（カメラ）サイズに引き伸ばしてモニタに写し出される)
        context.ExecuteCommandBuffer(commandBuffer);//コマンド設定
        context.Submit();//コマンド実行
        
        CommandBufferPool.Release(commandBuffer);
    }


    // ②描画タイミングを決めるところ 
    /// <summary>Constructor</summary>
    public DownSamplingRenderPass() => renderPassEvent = RenderPassEvent.AfterRenderingTransparents;//RenderPassEventはEnum  RenderPassEventにはさまざまな描画タイミングが定義されており、変数「renderPassEvent」に指定することでPassの実行タイミングを設定できます
    //「AfterRenderingTransparents」 を指定しました。 「透明オブジェクトを描画した後に実行する」 という処理


    /// <summary> Execute実行前のパラメータを渡すメソッド </summary>
    public void SetParam(RenderTargetIdentifier renderTarget, int downSample)
    {
        _currentTarget = renderTarget;
        _downSample = downSample;
        if (_downSample <= 0) _downSample = 1;
    }
}