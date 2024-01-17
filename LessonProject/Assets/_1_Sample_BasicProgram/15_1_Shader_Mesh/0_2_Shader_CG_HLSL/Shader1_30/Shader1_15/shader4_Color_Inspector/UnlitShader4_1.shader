Shader "Unlit/UnlitShader4_1"
{
    Properties
    {
        //ここに書いたものがInspectorに表示される
        _RedValue("Red Value", float) = 0.5
        _GreenValue("Green Value", float) = 0.5
        _BlueValue("Blue Value", float) = 0.5
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
            float _RedValue;
            float _GreenValue;
            float _BlueValue;


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


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;//頂点に関してはいつも通り3Dを2Dに変換して適用
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //RGBAにそれぞれのプロパティを当てはめてみる
                return float4(_RedValue, _GreenValue, _BlueValue, 1);
            }
            ENDCG
        }
    }
}
