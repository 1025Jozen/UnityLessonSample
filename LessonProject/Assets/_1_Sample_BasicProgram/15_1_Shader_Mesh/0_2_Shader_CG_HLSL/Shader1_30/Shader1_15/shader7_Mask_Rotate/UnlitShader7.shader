Shader "Unlit/UnlitShader7"
{
    Properties//Inspectorに出すプロパティー
    {     
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}          //テクスチャー(オフセットの設定なし)　こっちだけ回す
        [NoScaleOffset] _MaskTex("Mask Texture (RGB)", 2D) = "white" {}//Mask用テクスチャー        　      こっちは固定
        _RotateSpeed ("Rotate Speed", float) = 1.0   //回転の速度
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

            /*        */

            struct appdata  //頂点シェーダーに渡ってくる頂点データ
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0; //1番目のテクスチャUV座標
                float2 uv2 : TEXCOORD1; //2番目のテクスチャUV座標
            };

            struct v2f   //フラグメントシェーダーへ渡すデータ
            {
                float2 uv1 : TEXCOORD0; //1番目のテクスチャUV座標
                float2 uv2 : TEXCOORD1; //2番目のテクスチャUV座標
                float4 vertex : SV_POSITION; //座標変換された後の頂点座標
            };

            /*        */

            sampler2D _MainTex;
            sampler2D _MaskTex;
            fixed _RotateSpeed;

            /*        */

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //3D空間座標→スクリーン座標変換
                o.uv1 = v.uv1;//uvデータをそのまま渡す
                o.uv2 = v.uv2;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                /*回転*/
                half timer = _Time.y;// Timeを入力として現在の回転角度を作る

                // 時間で回転する行列を作るための下準備
                half angleCos = cos(-1* timer * _RotateSpeed);//右回転にするため -1かけてる
                half angleSin = sin(-1* timer * _RotateSpeed);//右回転にするため -1かけてる

                /*       |cosΘ -sinΘ|
                  R(Θ) = |sinΘ  cosΘ|  2次元回転行列の公式*/
                half2x2 rotateMatrix = half2x2(angleCos, -angleSin, angleSin, angleCos);
                
                half2 uv1 = i.uv1 - 0.5;//座標の原点である(0,0)とUV座標の中心を合わせる　( 全体的に　x座標y座標を　-0.5して　画像をずらす  )
                
                i.uv1 = mul(uv1, rotateMatrix) + 0.5;//uv座標(0.5,0.5)を起点にメインテクスチャのUVを回転させる
                                                     //mul 行列乗算　uv座標をrotateMatrixで回転させるという順　 


                /*マスク設定*/
                fixed4 mask = tex2D(_MaskTex, i.uv2);// マスク用画像のピクセルの色　取得
                
                clip(mask.a - 0.5);//引数の値が"0以下なら"描画しない　すなわち"Alphaが0.5以下なら"描画しない
                

                /*色設定*/
                fixed4 col = tex2D(_MainTex, i.uv1);//メインテクスチャーの色 取得


                return col * mask;//メイン画像とマスク画像のピクセルの計算結果を掛け合わせる　0に何をかけても0
            }
            ENDCG
        }
    }
}
