Shader "Unlit/UnlitShader10_2"
{
    Properties
    {        
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}//スクロールさせるテクスチャ        
        _Color("MainColor",Color) = (0,0,0,0)//色  
        _SliceSpace("SliceSpace",Range(0,30)) = 15//スライスされる間隔
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType"="Opaque"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //変数の宣言　Propertiesで定義した名前と一致させる
            sampler2D _MainTex;
            half4 _Color;
            half  _SliceSpace;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 localPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;         
                o.localPos = v.vertex.xyz;//描画しようとしている頂点(ローカル座標)
                o.uv = v.uv + _Time;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);//テクスチャの色を取得
                
                clip(frac(i.localPos.y * _SliceSpace) - 0.5);
                //各頂点のローカル座標(Y軸)それぞれに15をかけてfrac関数で少数だけ取り出す
                //そこから-0.5してclip関数で0を下回ったら描画しない

                
                return half4(col * _Color);
                //テクスチャの色とプロパティで設定した色を乗算する
                //0 0 0 　が黒を表すので　黒を設定するとUVがスクロールしているように見えない
            }
            ENDCG
        }
    }
}