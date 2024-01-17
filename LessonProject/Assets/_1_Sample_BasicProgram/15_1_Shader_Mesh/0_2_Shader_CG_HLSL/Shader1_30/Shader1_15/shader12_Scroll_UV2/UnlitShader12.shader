Shader "Unlit/UnlitShader12"
{
    Properties
    {
        //色
        _StripeColor1("StripeColor1",Color) = (1,0,0,0)
        _StripeColor2("StripeColor2",Color) = (0,1,0,0)
        
        _SliceSpace("SliceSpace",Range(0,20)) = 15       //スライスされる間隔
        _SliceBlend("SliceBlend",Range(0,1)) = 0.5       //２色の割合

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
            half4 _StripeColor1;
            half4 _StripeColor2;
            
            half  _SliceSpace;
            half  _SliceBlend;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;              
                o.uv = v.uv + _Time.x * 2;   //UVスクロール
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {              
                half interpolation = step(frac(i.uv.y * _SliceSpace), _SliceBlend);
                //frac関数で小数点だけ取り出す _SliceSpaceの間隔で規則性が生まれる 0以上1未満の値が返る

                //補間値の計算　        
                //step関数：step(t, x)
                //xの値がtよりも小さい場合には0、大きい場合には1を返す

                //
                
                half4 color = lerp(_StripeColor1,_StripeColor2, interpolation);//interpolationの値によってColor1かColor2のどちらかを返す
                return color;//処理後、ピクセルの色を返す
            }
            ENDCG
        }
    }
}
