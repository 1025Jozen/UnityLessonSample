Shader "UnlitShader22_2_Sun"
{
    Properties {
        _BGColor  ("Background Color", Color) = (0.7, 0.7, 1, 1)//背景色
        _SunColor ("Color", Color)            = (1, 0.8, 0.5, 1)//太陽の色
        _SunDir   ("Sun Direction", Vector)   = (0, 0.5, 1, 0)  //太陽の位置
        _SunStrength("Sun Strengh", Range(0, 30)) = 12          //光の強さ
    }
    SubShader
    {
        Tags
        {
            "RenderType" ="Background"
            "Queue"      ="Background"
            "PreviewType"="SkyBox"
        }

        Pass
        {
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed3 _BGColor;
            fixed3 _SunColor;
            float3 _SunDir;
            float _SunStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            /*       */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 dir  = normalize(_SunDir);   //設定した太陽の位置　ベクトル正規化
                float angle = dot(dir, i.texcoord); //太陽の位置ベクトル　と　描画ピクセルの位置ベクトル　の内積(-1~1で角度を表現する感じ　　1だと太陽の方を向いてる　)
                
                fixed3 c = _BGColor + _SunColor * pow(max(0, angle), _SunStrength);
                //背景色　と　　　　「(pow(max(0, angle) →　０〜１ )を　_SunStrength乗　する 」これも0~1の値　*  _SunColor  　 _SunStrengthの値が大きいほど　数値が小さくなっていく

                //pow(x,y)はxをy乗する　
                //0 < max(0, angle) < 1 なので　_SunStrengthを大きくするほど計算結果は0に近づく
                //色にマイナス値はない

                return fixed4(c, 1.0);
            }
            ENDCG
        }
    }
}
