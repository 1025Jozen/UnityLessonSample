using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URP_Shader_Sample1_RendererFeature : ScriptableRendererFeature
{
    private URP_Shader_Sample1_Pass _pass;
    [SerializeField] private Shader _shader;
    public Color color;

    //①Passの作成
    public override void Create(){
        var material = CoreUtils.CreateEngineMaterial(_shader);//ShaderからMaterialを生成   再生終了時に自動でMaterialの破棄もしてくれます。 
        _pass = new URP_Shader_Sample1_Pass(material);         // PassにMaterialを渡す 
    }


    //②生成したPassをScriptableRendererに渡す
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData){    
        renderer.EnqueuePass(_pass);
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData) {
        _pass.SetParam(renderer.cameraColorTargetHandle, color);
    }
}