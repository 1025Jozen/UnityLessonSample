Shader "Unlit/UnlitShader5"
{
    Properties
    {       
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {} //テクスチャー(オフセット、タイリングの設定なし)   
        _RotateSpeed ("Rotate Speed", float) = 1.0            //回転の速度
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata //頂点シェーダーに渡ってくる頂点データ(構造体)
            {
                //  :以下は　　 この変数にはポジションを入れてあるよ！みたいなのをGPUに教えてる
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;    //1番目のUV座標 を uvが受け取る
            };

            struct v2f            //(フラグメントシェーダーへ渡すデータ)
            {
                float2 uv : TEXCOORD0;      //テクスチャUV  
                float4 vertex : SV_POSITION;//座標変換された後の頂点座標
            };

            sampler2D _MainTex;
            float _RotateSpeed;

            /*                     */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);//3D座標をモニター座標に変換
                o.uv = v.uv; //UV座標を渡す
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Timeを入力として現在の回転角度を作る
                half timer = _Time.x;

                // 時間によって書いてする 回転行列を作っていく
                half angleCos = cos(timer * _RotateSpeed);
                half angleSin = sin(timer * _RotateSpeed);

                /*       |cosΘ -sinΘ|
                  R(Θ) = |sinΘ  cosΘ|  2次元回転行列の公式*/
                half2x2 rotateMatrix = half2x2(angleCos, -angleSin, angleSin, angleCos);


                //座標の原点である(0,0)とUV座標の中心を合わせる　( 全体的に　x座標y座標を　-0.5して　画像をずらす  )
                half2 uv = i.uv - 0.5;//  half2 uv = i.uv;でやると違う結果に(「画像ずらすイメージ.png」参照)

                i.uv = mul(uv, rotateMatrix) + 0.5;//uv座標(0.5,0.5)を起点にメインテクスチャのUVを回転させる
                                                   //mul 行列乗算　uv座標をrotateMatrixで回転させるという順　 

                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
