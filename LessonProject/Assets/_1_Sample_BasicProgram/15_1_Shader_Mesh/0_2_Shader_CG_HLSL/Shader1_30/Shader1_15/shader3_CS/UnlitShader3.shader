Shader "Unlit/UnlitShader3"
{
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

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half4 color : COLOR0;
            };


            v2f vert (appdata_full v)
            {
                v2f o; //構造体のオブジェクトを作る
                o.vertex = UnityObjectToClipPos(v.vertex);//変換
                o.color = v.color;//(c#の方で記述した)頂点に設定された色を適用
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
                //頂点の色情報をそのまま利用する
                //頂点と頂点の間の色はUnityで勝手に補間してくれている(ラスタライズ)
            }
            ENDCG
        }
    }
}
