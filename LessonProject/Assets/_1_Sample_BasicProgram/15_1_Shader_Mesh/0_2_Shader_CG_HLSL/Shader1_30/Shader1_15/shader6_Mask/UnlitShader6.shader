Shader "Unlit/UnlitShader6"
{
    Properties
    {
        //テクスチャー(オフセットの設定なし)
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        
        //Mask用テクスチャー(オフセットの設定なし)
        [NoScaleOffset] _MaskTex("Mask Texture (RGB)", 2D) = "white" {}
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

            sampler2D _MainTex;//描画
            sampler2D _MaskTex;//この画像に合わせて描画しないを設定する

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //マスク用画像のピクセルの色を取得
                fixed4 mask = tex2D(_MaskTex, i.uv);
                
                clip(mask.a - 0.5);
                //clipの引数に渡した値が0以下となった場合、描画しない
                //今回は"Alphaが0.5以下なら"描画しない
                //(マスク画像の描画したくない部分のAlpha値は,あらかじめ0.5以下になるように透過した画像を用意しておく必要があります。)

                
                fixed4 col = tex2D(_MainTex, i.uv);//メイン画像のピクセルの色を取得
               
                return col * mask; //メイン画像とマスク画像のピクセルの計算結果を掛け合わせる　0に何をかけても0
            }
            ENDCG
        }
    }
}
