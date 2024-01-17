Shader "Unlit/UnlitShader10_1"
{
    Properties
    {
        //スクロールさせるテクスチャ
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //変数の宣言　Propertiesで定義した名前と一致させる
            sampler2D _MainTex;
            half _SliceSpace;

            struct appdata  //GPUから頂点シェーダーに渡す構造体
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f //頂点シェーダーからフラグメントシェーダーに渡す構造体
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD1;
            };


            v2f vert (appdata v)
            {
                v2f o;                
                o.uv.y = v.uv.y + _Time.y;  //UVに時間を足してく //表示しているUVの枠が上に移動(絵柄は下に移動しているように見える) //UVをスクロールさせている箇所
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 col = tex2D(_MainTex, i.uv); //テクスチャとUV座標から色を取得
                //　頂点シェーダーから渡ってきたこのUVが時間で変化する
                // tex2D関数は、UV座標(uv_MainTex)からテクスチャ(_MainTex)上のピクセルの色を返します。

                return half4(col);
            }
            ENDCG
        }
    }
}
