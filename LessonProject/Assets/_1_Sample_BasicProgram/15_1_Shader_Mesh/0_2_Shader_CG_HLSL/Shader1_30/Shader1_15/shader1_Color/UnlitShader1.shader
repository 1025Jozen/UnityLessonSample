Shader "Unlit/UnlitShader1"   //Shaderの名前
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }//タグ　透明度とか設定できる
        LOD 100

        Pass
        {
            
            CGPROGRAM //こっからCG言語で書きますよみたいな宣言

            //vertexシェーダーとfragmentシェーダーの関数がどれなのか伝える
            #pragma vertex vert  //vertという名前の関数がvertexシェーダーです　  と宣言 
            #pragma fragment frag//fragという名前の関数がfragmentシェーダーです　と宣言
            
            #pragma multi_compile_fog // make fog work

            #include "UnityCG.cginc"  //便利関数詰め合わせセット

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f //v2fという構造体を定義　Vertex to Fragment の略
                       //(vertexシェーダーとfragmentシェーダーの間におけるデータのやりとりで使う)
                       //vertexシェーダーの結果をfragmentシェーダーに渡す
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;//位置情報 　"：" 以降の大文字はセマンティクスと言って必要なものだけ受け取るために用意されてい
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            /*          */
            // "これがvertexシェーダーです"　と宣言した関数
            v2f vert (appdata v)//頂点の情報が引数に渡ってくる
            {
                v2f o;//先ほど宣言した構造体のオブジェクトを作る
                o.vertex = UnityObjectToClipPos(v.vertex);//"3Dの世界での座標は2D(スクリーン)においてはこの位置になりますよ"　という変換を行っている
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;//変換した座標を返す
            }

            // "これがfragmentシェーダーです"　と宣言した関数
            fixed4 frag (v2f i) : SV_Target //頂点シェーダからの入力(input)が引数に渡ってくる
            {
                fixed4 col = tex2D(_MainTex, i.uv);//colorを扱いたい RGBA float4 color = float4(1,1,0,1);
               
                UNITY_APPLY_FOG(i.fogCoord, col);// apply fog

                // return col;//色情報を返す　R G B A
                return half4(1, 0, 0, 1);//赤！と指定した　　return half4(1, 0, 0, 1);なら　黄！
            }


            ENDCG
        }
    }
}
