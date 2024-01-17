using UnityEngine;

public class Paintable : MonoBehaviour
{
    const int TEXTURE_SIZE = 1024;

    public float extendsIslandOffset = 1;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;
    
    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");//PaintableシェーダーのTextureID　(PaintableObjectにはPaintableマテリアルをつけてる)

    //どれもStartで新規生成
    public RenderTexture getMask()      => maskRenderTexture;         //マスク 部分にペイントを施す
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;    //uv アイランアイランド 　テクスチャ
    public RenderTexture getExtend()    => extendIslandsRenderTexture;//　  アイランド　
    public RenderTexture getSupport()   => supportTexture;            //サポート用テクスチャの用意
    public Renderer getRenderer()       => rend;//このクラスをつけたファイル(インスタンス化された)ObjectについているRenderコンポーネントのアドレスをgetするためのプロパティ

    void Start() {
        //初期設定

        //テクスチャ生成
        maskRenderTexture            = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture            = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture            = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        supportTexture.filterMode = FilterMode.Bilinear;



        extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        //コンポーネントアドレス取得
        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);//ShaderのmaskTextureに出来立てテクスチャを適応

        //初期化
        PaintManager.instance.initTextures(this);
    }

    //Objectが無効化されたら　テクスチャを破棄
    void OnDisable(){
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }
}