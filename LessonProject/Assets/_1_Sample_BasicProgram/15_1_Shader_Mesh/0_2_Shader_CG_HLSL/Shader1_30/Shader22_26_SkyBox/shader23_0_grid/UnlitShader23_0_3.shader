Shader "Unlit/UnlitShader23_0_3"
{
    Properties
    {
        
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

            /*     */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            /*     */

            float4 frag (v2f i) : SV_Target
            {                
                float dx = 0.5 - i.uv.x;
                float dy = 0.5 - i.uv.y;
                float rad = atan2(dx, dy);
                rad = rad * 180 / UNITY_PI;//ここで度数法に直す πで割って180で掛ける
                rad = rad +180;            //ついでに真上を0度(360度)にする

                return rad;//1なら白　大体が１〜360度で白になるはず　ただ0~1度のところで若干黒くなってるはず
            }
            ENDCG
        }
    }
}
