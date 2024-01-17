Shader "Unlit/UnlitShader4_2"
{
    Properties
    {
        //ここに書いたものがInspectorに表示される
        _Color("MainColor",Color) = (0,0,0,0)
    }

    SubShader
    {
         //不当明度を利用するときに必要 
        Tags { "RenderType"="Tranparent" }
        Blend SrcAlpha OneMinusSrcAlpha  
        //1 - フラグメントシェーダーのAlpha値(不透明度)　という意味
        //SrcAlphaは フラグメントシェーダから出力されたα値
        //OneMinusSrcAlphaは 1 – フラグメントシェーダから出力されたα値
        //最終的に出力される色 → フラグメントシェーダの出力 * α値 + 既に画面に描画されている色 * (1 – α値）

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //変数の宣言　Propertiesで定義した名前と一致させる
            half4 _Color;

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };


            v2f vert(appdata_base v) 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //RGBAにそれぞれのプロパティを当てはめてみる
                return half4(_Color);
            }
            ENDCG
        }
    }
}
