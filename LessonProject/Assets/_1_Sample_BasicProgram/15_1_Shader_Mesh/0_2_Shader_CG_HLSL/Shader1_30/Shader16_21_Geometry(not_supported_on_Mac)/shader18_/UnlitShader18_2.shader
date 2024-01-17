Shader "Unlit/18_2"
{
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            appdata vert(appdata v)
            {
                return v;
            }

            struct g2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            //ランダムな値を返す
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            // ジオメトリシェーダー
            [maxvertexcount(3)]
            void geom(triangle appdata input[3],inout TriangleStream<g2f> stream)
            {
                //[unroll]以後はポリゴンの各頂点に対して行う処理を記述している箇所
                //その中でランダムな色を定義して各頂点に対して色を当てはめています。

                //全ての頂点にランダムな色を当てはめているので
                //頂点と頂点の間をラスタライズして
                //グラデーションのようになっているのがわかります。
                [unroll]
                for (int i = 0; i < 3; i++)
                {
                    appdata v = input[i];
                    g2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);


		            //ランダムな値
                    float r = rand(v.vertex.xy);
                    float g = rand(v.vertex.xz);
                    float b = rand(v.vertex.yz);
                    o.color = fixed4(r,g,b,1);
                    stream.Append(o);
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}