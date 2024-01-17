Shader "Unlit/StencilWrite"
{
     Properties
    {
        //"深度値の比較によってピクセルを上書きするか定めるフロー" ZTest
        [KeywordEnum(OFF,ON)] _ZWrite("ZWrite",Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4

        //今回は 深度　書き込まないので　ZWrite off
  
        //Unity の ZWrite は、レンダリング時に深度バッファのコンテンツを更新するかどうかを設定します。
        //ZWrite は基本的に「深度情報」  カメラ（視点）から見た各オブジェクトの距離です。﻿
        //ZWrite は通常、不透明なオブジェクトには有効で、半透明なオブジェクトには無効です。
        //半透明の効果を描画する場合は ZWrite Off にします。﻿
        //ZWrite を有効にするには、シェーダの「ZWRITE」を「On」に切り替えます。デフォルトでは On になっています


        //描画順　ZWrite がONなら基本
        //小さければ小さいほど、基本的には先にレンダリングされる 。表示される順番 を直接的に指しているわけではない
        //Backgroundが1000、Geometryが2000、AlphaTestが2450、Transparentが3000、Overlayが4000
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZWrite [_ZWrite]
            ColorMask 0

            // ステンシルバッファの設定
            Stencil{
                Ref 2       // ステンシルの番号 2という番号を持ってますよという意味　　基本的に描画されているもののRefは 0 と設定されている
                Comp Always //このシェーダでレンダリングされたピクセルのステンシルバッファを「対象」とする   // NotEqual: ステンシル番号がバッファ値と等しくないステンシル番号のピクセルのみレンダリング
                Pass Replace// Replace: 「対象」としたステンシルバッファにRefの値を書き込む
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return 1;
            }
            ENDCG
        }
    }
}
