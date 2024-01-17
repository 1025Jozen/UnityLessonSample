using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URP_Shader_Sample1_Pass : ScriptableRenderPass
{
    private const string CommandBufferName = nameof(URP_Shader_Sample1_Pass);
    private readonly int RenderTargetTexId = Shader.PropertyToID("_RenderTargetTex");
    
    private RenderTargetIdentifier _currentRenderTarget;
    private readonly Material _material;

    public URP_Shader_Sample1_Pass(Material material)
    {
        _material = material;
        renderPassEvent = RenderPassEvent.BeforeRenderingSkybox;
    }

    public void SetParam(RenderTargetIdentifier target, Color color)
    {
        _material.color = color;
        _currentRenderTarget = target;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var commandBuffer = CommandBufferPool.Get(CommandBufferName);
        var cameraData = renderingData.cameraData;
        var w = cameraData.camera.scaledPixelWidth;
        var h = cameraData.camera.scaledPixelHeight;

        
        commandBuffer.GetTemporaryRT(RenderTargetTexId, w, h, 0, FilterMode.Bilinear); //RenderTexture生成      
        commandBuffer.Blit(_currentRenderTarget, RenderTargetTexId, _material);//カメラ画像をRenderTextureにコピー//CommandBuffer.Blit(元の画像,書き出し画像,適用シェーダー)// ApplyShader     
        commandBuffer.Blit(RenderTargetTexId, _currentRenderTarget);//RenderTextureをカメラ画像にコピー// Back RenderTarget
        commandBuffer.ReleaseTemporaryRT(RenderTargetTexId);//臨時テクスチャ破棄        
        context.ExecuteCommandBuffer(commandBuffer);        //コマンド設定
        context.Submit();                                   //コマンド実行
               
        CommandBufferPool.Release(commandBuffer);//コマンドプール内リセット
    }
}
