Shader "Unlit/UnlitShader23_0_8"
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
            float grad_rot(float2 st , float grad_length) {
                float dx = 0.5 - st.x;
                float dy = 0.5 - st.y;

                float rad = atan2(dx, dy);
                rad = rad * 180 / UNITY_PI + 180;

                float n = floor((_Time * 1000) / 360);
                float offset = _Time * 1000;
                rad = rad + n * 360;

                float d1 = distance(rad, offset) / grad_length;      //現在 
                float d2 = distance(rad, offset + 360) / grad_length;//追い越し
                float d3 = distance(rad, offset - 360) / grad_length;//遅れ

                return min(min(d1, d2), d3);//min関数：小さいほうの値を返す
            }

            float4 frag(v2f i) : SV_Target {
                return grad_rot(i.uv , 50);
            }
            ENDCG
            
        }
    }
}
