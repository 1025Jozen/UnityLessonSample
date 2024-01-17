Shader "Unlit/UnlitShader9"
{
    Properties//ここに書いたものがInspectorに表示される
    {  
        _Color("MainColor",Color) = (0,0,0,0)        
        _SliceSpace("SliceSpace",Range(0,30)) = 15//スライスされる間隔
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //変数の宣言　Propertiesで定義した名前と一致させる
            half4 _Color;
            half _SliceSpace;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 localPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_base v)
            {
                v2f o;                
                o.localPos = v.vertex.xyz;//描画しようとしている頂点の座標(ローカル座標)を取得
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;//頂点自体はいじってない
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //各頂点のローカル座標(Y軸)それぞれに_SliceSpaceをかけてfrac関数で少数だけ取り出す
                //そこから-0.5してclip関数で0を下回ったら描画しない
                //定期的に0が続くようになる
                clip(frac(i.localPos.y * _SliceSpace) - 0.5);
                
                return half4(_Color);//RGBAを当てはめる
                //Objectを動かしても縞模様は動かなくなった
            }
            ENDCG
        }
    }
}
