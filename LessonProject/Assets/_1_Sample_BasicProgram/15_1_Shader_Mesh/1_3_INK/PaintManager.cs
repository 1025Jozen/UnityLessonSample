using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PaintManager : Singleton<PaintManager>
{
    public Shader texturePaint;
    public Shader extendIslands;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID  = Shader.PropertyToID("_PainterPosition");
    int hardnessID  = Shader.PropertyToID("_Hardness");
    int strengthID  = Shader.PropertyToID("_Strength");
    int radiusID    = Shader.PropertyToID("_Radius");
    int colorID     = Shader.PropertyToID("_PainterColor");
    int textureID   = Shader.PropertyToID("_MainTex");
    int uvOffsetID  = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    Material paintMaterial;
    Material extendMaterial;

    CommandBuffer command;



    public override void Awake(){
        base.Awake();
        
        paintMaterial  = new Material(texturePaint); //シェーダーを適応したマテリアル生成
        extendMaterial = new Material(extendIslands);//シェーダーを適応したマテリアル生成
        command        = new CommandBuffer();        //コマンドバッファを使用するためインスタンス化
        command.name   = "CommmandBuffer - " + gameObject.name;
    }

    public void initTextures(Paintable paintable){
        //Paintableクラス　の各アドレスから　情報取得
        RenderTexture mask      = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend    = paintable.getExtend();
        RenderTexture support   = paintable.getSupport();
        Renderer rend           = paintable.getRenderer();

        //
        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);//実行
        command.Clear();
    }

    //ここを呼び出してペイントを行う
    public void paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null){
        RenderTexture mask      = paintable.getMask();     //
        RenderTexture uvIslands = paintable.getUVIslands();//Objectの新規UVアイランド
        RenderTexture extend    = paintable.getExtend();  // アイランド
        RenderTexture support   = paintable.getSupport(); //
        Renderer rend           = paintable.getRenderer();//ObjectのRenderコンポーネントのアドレス

        //texturePaint シェーダーに値を渡す
        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);          //どこを中心にお絵描きするか
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support);       //texturePaint に　 サポートテクスチャ(今描いたやつ)を渡す　　今描いたやつと今まで描いたやつを合成する
        paintMaterial.SetColor(colorID, color ?? Color.red);//

        //extendMaterial　シェーダーに値を渡す
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);//
        extendMaterial.SetTexture(uvIslandsID, uvIslands);                 //

        //CommandBuffer　レンダリングパイプラインの特定の段階でカスタムのグラフィックス命令を発行できます
        command.SetRenderTarget(mask);               //描画対象のレンダーターゲットを設定 
        command.DrawRenderer(rend, paintMaterial, 0);//CommandBufferによってレンダラーを描画します。特定のレンダラーを描画するために使用され、オブジェクトのレンダリング命令を発行　　　//RenderコンポーネントにpaintMaterialを　　

        command.SetRenderTarget(support);//描画対象のレンダーターゲットを設定 　　　サポートテクスチャ
        command.Blit(mask, support);     //maskで描いた内容を　supportに保持

        command.SetRenderTarget(extend);//描画対象のレンダーターゲットを設定
        command.Blit(mask, extend, extendMaterial);//CommandBuffer.Blit(元の画像,書き出し画像,適用シェーダー)// //マスク を　extendMaterialextend　に合成

        Graphics.ExecuteCommandBuffer(command);//CommandBuffer内の命令を実行します。このメソッドは通常、カメラがレンダリングする際に呼び出されます。
        command.Clear();
    }

}
