Shader "test/Plain"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}//モデルのテクスチャ
        _Threshold("Threshold", Range(-1,1)) = 1//どれくらいの高さより高い位置を透明にするかを決定する値
        _LazerTex("LazerTexture", 2D) = "white"{}//透明部分と不透明部分の境目のレーザーのテクスチャ
        _LazerHeight("LazerHeight", Range(0,1)) = 0.1//レーザーの太さを決める値
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}//透明なオブジェクトを描画
        LOD 100
        Cull off   //ポリゴンの裏側は描画しない
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            //Unityシェーダーのコードは実はシェーダー言語が記述されている部分はCGPROGRAMからENDCGまで
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION; //頂点座標
                float2 uv     : TEXCOORD0;//テクスチャ用のUV座標
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float3 oPos   : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Threshold;
            sampler2D _LazerTex;
            float _LazerHeight;


            //頂点
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);//座標変換は、オブジェクト座標→ワールド座標→ビュー座標→クリップ座標の順に行われますが、これをまとめて処理
                o.uv     = v.uv;
                o.oPos   = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float diff = _Threshold - i.oPos.y;//uv座標のv座標
                clip(diff);//0未満の値が入るとそのピクセルの描画を放棄する(=透明にする)関数  _Thresholdより大きい場合に透明にします。
                if(diff < _LazerHeight){//境目からの距離がこの値より小さければレーザーのテクスチャを重ねます。
                    fixed4 lazerCol = tex2D(_LazerTex, diff / _LazerHeight);
                    col.rgb = col.rgb * (1 - lazerCol.a) + lazerCol.rgb * col.a;
                }

                //レーザーのテクスチャから色をサンプリングしてlazerColに代入します。
                //第2引数にfloat2ではなくfloatを渡していますが、これはfloat2(diff / _LazerHeight, diff / _LazerHeight)と同等です。
                //レーザーのテクスチャは横に引き伸ばしたような画像なのでx座標は特に意味はないです。
                //Y座標は、diffが0の時に0、diffが_LazerHeightと等しい時に1になるようにしたいので、diffを_LazerHeightで除算しています。
                //レーザーのテクスチャはY座標が0.5付近の時は不透明ですが、0と1に近づくにつれて透明になるようにしてあります。
                //なので下地になる_MainTexの色とレーザーの色を適当にブレンド(混ぜる)ます。
                //レーザーのテクスチャの透明度が低い(不透明)場合ほどレーザーのテクスチャの色が_MainTexより優先されるようにブレンドしています。

                return col;
            }
            ENDCG
        }
    }
}
