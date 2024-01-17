Shader "Unlit/UnlitShader2"
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)  //vはメッシュの頂点情報
            {
                v2f o;   //構造体のオブジェクトを作る            
                // float4 vert = v.vertex * 0.75;        //メッシュの頂点座標を0.75倍している→縮小
                                                      //float4の理由は　3D→2D変換を実行する際　に役立つからくらいで
                                                      //初めの３つはxyz　4つ目wは同次座標 で通常１　　同次座標(回転　拡大縮小　移動　を１つの行列で表す際に必要となる要素くらいで)

                float4 vert = float4(v.vertex.xyz * sin(_Time.y), v.vertex.w);//sin波で座標をいじってる
                //メッシュの頂点座標を時間経過に応じてSin関数で変化させている
                //_Time.yの理由　　 _Timeはロードからの時間を表すfloat4 (t/20, t, t*2, t*3)。シェーダー内でアニメーション化を行うために使用します。


                o.vertex = UnityObjectToClipPos(vert);//3Dを2D(モニター座標)に変換
                return o;//変換した座標を返す
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return half4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
