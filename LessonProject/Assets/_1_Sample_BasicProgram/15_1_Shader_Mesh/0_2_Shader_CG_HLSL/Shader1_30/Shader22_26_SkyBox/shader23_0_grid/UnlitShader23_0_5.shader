Shader "Unlit/UnlitShader23_0_5"
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
                float rad = atan2(dx, dy); //座標によって中心からの角度を求める
                rad = rad * 180 / UNITY_PI;//ここで度数法に直す πで割って180で掛ける
                rad = rad + 180;           //ついでに真上を0度(360度)にする

                float n      = floor((_Time * 1000) / 360);//floor関数で整数部を取り出す　
                float offset = _Time * 1000 - n * 360;     //1000m秒　= 1秒
                float s      = step(rad, offset);          //radがoffset以下ならば1を返す 
                //360で周期させる _Time * 1000 - n * 360　のおかげで　360以内の値になる　

                return s;//1なら白
            }
            ENDCG
        }
    }
}
