Shader "Unlit/UnlitShader23_0_6"
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

            float4 frag(v2f i) : SV_Target {
                float2 st = i.uv;
                float dx = 0.5 - st.x;
                float dy = 0.5 - st.y;

                float rad = atan2(dx, dy);
                rad = rad * 180 / UNITY_PI + 180;

                float n1 = floor((_Time * 1000 + 60) / (360));//６０°分周回をずらす
                float n2 = floor((_Time * 1000) / 360);
                float offset1 = _Time * 1000 - n1 * 360;//n1ですでにずらしたのでそのまま
                float offset2 = _Time * 1000 - n2 * 360;
                float s1 = step(rad, 60 + offset1);//６０°から回転
                float s2 = step(offset2, rad);//０°から回転
                //shaderでif文は使わない方がいい

                return (s1*s2)*step(offset2, 360 - 60) + (s1 + s2)*(1-step(offset2, 360 - 60));
            }
            ENDCG
            
        }
    }
}
