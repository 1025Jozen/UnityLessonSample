Shader "Unlit/UnlitShader23_0_7"
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
            float disc(float2 st) {
                float d = distance(st, float2(0.5, 0.5));//中心からの距離
                float s = step(0.5, d);                  //0.5 < d　なら 1
                return s;//
            }

            float4 frag(v2f i) : SV_Target {
                float2 st = i.uv;
                float dx = 0.5 - st.x;  //中心からの距離
                float dy = 0.5 - st.y;  //中心からの距離

                float rad = atan2(dx, dy);        //
                rad = rad * 180 / UNITY_PI + 180; //度数法に変換　して　上が0度になるように

                float n1 = floor((_Time * 1000 + 60) / (360));//６０°分周回をずらす
                float n2 = floor((_Time * 1000) / 360);
                float offset1 = _Time * 1000 - n1 * 360;      //n1ですでにずらしたのでそのまま　360以内
                float offset2 = _Time * 1000 - n2 * 360;
                float s1 = step(rad, 60 + offset1);           //６０°から回転　60 + offset1なら黒
                float s2 = step(offset2, rad);                //０°から回転   offset2がradを超えたら　

                return (s1 * s2) * step(offset2, 360 - 60)       //0*0 0 0*1 0  1*1 1  300
                     + (s1 + s2) * (1 - step(offset2, 360 - 60)) //0+0   0+1 1  1+1 2
                     - disc(st); //円形にマスク
            }
            ENDCG
            
        }
    }
}
