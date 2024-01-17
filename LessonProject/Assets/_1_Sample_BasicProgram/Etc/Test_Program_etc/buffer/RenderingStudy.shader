Shader "Custom/RenderingStudy"
{
    Properties
    {
        //"深度値の比較によってピクセルを上書きするか定めるフロー"をZTest
        _Color("MainColor",Color) = (0,0,0,0)
        [KeywordEnum(OFF,ON)] _ZWrite("ZWrite",Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTest("ZTest", Float) = 4

        //描画順　ZWrite がON(半透明には無効)なら基本
        //小さければ小さいほど、基本的には先にレンダリングされる 。表示される順番 を直接的に指しているわけではない
        //Backgroundが1000、Geometryが2000、AlphaTestが2450、Transparentが3000、Overlayが4000
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue"      = "Geometry-1"
        }
        
        Pass
        {
            ZWrite [_ZWrite]  //
            ZTest  [_ZTest]
            //Less	    描画済みのピクセルの深度値 > 描画予定のピクセルの深度値
            //LEqual	描画済みのピクセルの深度値 ≧ 描画予定のピクセルの深度値
            //Equal	    描画済みのピクセルの深度値 = 描画予定のピクセルの深度値
            //GEqual	描画済みのピクセルの深度値 ≦ 描画予定のピクセルの深度値
            //Greater	描画済みのピクセルの深度値 < 描画予定のピクセルの深度値
            //NotEqual	描画済みのピクセルの深度値 ≠ 描画予定のピクセルの深度値
            //Always	深度値に関係なく合格

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            half4 _Color;

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            //頂点
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            //フラグメント
            half4 frag(v2f i) : COLOR
            {
                return half4(_Color);
            }
            ENDCG
        }
    }
}