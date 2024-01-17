Shader "Unlit/16"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _PositionFactor("Position Factor", float) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            //vertex → geometry → fragment　の順に　情報を　渡していく
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom //ジオメトリシェーダーの関数がどれかGPUに教える
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float _PositionFactor;

            /*  */
            //頂点シェーダーに渡ってくる頂点データ
            struct appdata
            {
                float4 vertex : POSITION;//座標について
            };

            //ジオメトリシェーダーからフラグメントシェーダーに渡すデータ
            struct g2f
            {
                float4 vertex : SV_POSITION;//スクリーン上の座標
            };


            /*    */
            //頂点シェーダー
            appdata vert(appdata v)
            {
                return v;//座標情報
            }

            //ジオメトリシェーダー
            //引数のinputは文字通り頂点シェーダーからの入力
            

            // ジオメトリシェーダでは3頂点のinputと3頂点のoutputを行う(つまり普通の三角ポリゴン)
            // [max vertex count (3)]   が   3頂点のoutputであることを伝えている
            // triangle appdata input[3]が  「三角形」で3頂点のinputが必要であることを伝えている
            // stream                   は   参照渡しで次の処理に値を受け渡ししている　TriangleStream<>で三角面を出力する (returnの代わりにinout)
            [maxvertexcount(3)] 
            void geom(triangle appdata input[3], inout TriangleStream<g2f> stream)
            {
                // 法線を計算
                float3 vec1 = input[1].vertex - input[0].vertex;//012 で三角形　　0→1　のベクトル　　0→2　のベクトル　
                float3 vec2 = input[2].vertex - input[0].vertex;
                float3 normal = normalize(cross(vec1, vec2));//外積から 法線取得

                [unroll] //繰り返す処理を畳み込んで最適化
                for (int i = 0; i < 3; i++)
                {
                    appdata v = input[i];
                    g2f o;                   
                    v.vertex.xyz += normal * (sin(_Time.w) + 0.5) * _PositionFactor;//法線ベクトルに沿って頂点(xyz座標)を移動
                    o.vertex = UnityObjectToClipPos(v.vertex);//3D→スクーン座標に変換
                    stream.Append(o);//リストに情報を入れる
                }
            }

            //フラグメントシェーダー
            fixed4 frag(g2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}