Shader "Unlit/UnlitShader11"
{
    Properties
    {       
        _Color("MainColor",Color) = (0,0,0,0)//色   
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;//v2f構造体　の変数
                
                o.uv = v.uv + _Time.y / 2 ;  
                // UVスクロール y成分に足してるので　uv座標が上に動く　
                //テクスチャ自体は下に動いているように見える (何枚もテクスチャが連なっているとして、uvが上に動くと模様が下に動くように見える)
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;//fragに渡される
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(frac(i.uv.y * _SliceSpace) - 0.5);//縞模様の描画はここ
                //各頂点のUV座標(Y軸)それぞれに_SliceSpaceをかけてfrac関数で少数だけ取り出す
                //そこから-0.5してclip関数で0を下回ったら描画しない　定期的に0が続く
                               
                return half4(_Color);//プロパティで設定した色を返す
            }
            ENDCG
        }
    }
}
