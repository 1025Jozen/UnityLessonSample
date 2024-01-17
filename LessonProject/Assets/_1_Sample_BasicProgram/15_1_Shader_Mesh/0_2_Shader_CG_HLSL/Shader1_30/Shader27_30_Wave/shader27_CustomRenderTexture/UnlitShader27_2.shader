Shader "Unlit/UnlitShader27_2"
{
    Properties
    {
        _CustomRendererTex("Custom Renderer Texture", 2D) = "gray" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _CustomRendererTex;


            //頂点シェーダー 　uv
            v2f vert(appdata v)
            {
                v2f o;// TEXCOORD0　テクスチャ　  SV_POSITION 画面座標
                o.vertex = UnityObjectToClipPos(v.vertex);//POSITION ゲーム内座標　を　画面座標　に
                o.uv     = v.uv;//TEXCOORD0   テクスチャ　uv 情報はそのまま渡す (fragで使う)
                return o;//頂点に関して
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //カスタムの方で書いたテクスチャを　そのまま　表示するために　カスタムテクスチャの　uv座標から色を取り出す
                return tex2D(_CustomRendererTex, i.uv);//カスタムレンダーテクスチャーのピクセル情報(uv座標)から該当座標の色を返す
            }
            ENDCG
        }
    }
}