Shader "Unlit/UnlitShader23_0_1"
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
            	float dx = 0.5 - i.uv.x;    //中心点とx座標の差
                float dy = 0.5 - i.uv.y;    //中心点とy座標の差
                float rad = atan2(dx, dy);  //上記作業により中心からの 角度が求められる
                                            //ただしラジアンで値が返っている　
                //0度は真下から始まっている　
                //時計回り　に度数が増えていき　//真上でπ
                //反時計回りで度数が減っていき　//真上で-π
                return rad;//マイナスでも黒　　０なら黒　1なら白　　ラジアン法における１は円弧の長さと半径と同じ長さ
            }
            ENDCG
        }
    }
}
