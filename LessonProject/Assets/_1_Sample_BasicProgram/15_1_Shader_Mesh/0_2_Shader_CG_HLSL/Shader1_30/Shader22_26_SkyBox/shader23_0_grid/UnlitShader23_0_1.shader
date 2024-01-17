Shader "Unlit/UnlitShader23_0_1"
{
    Properties
    {
        
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv     : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            /*     */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            /*     */
            float disc(float2 st, float size) {//uv座標のことを他にst座標と言ったりする
				float d = distance(st, float2(0.5, 0.5));//　中心からの距離　
				float s = step(d, size);//  中心からの距離 < size 　のときは1 それ以外は0
				return s;
			}

			float grid(float2 st) {//fragでは　0やマイナスだと黒　1なら白 
                //2つの円を組み合わせてる感じ
                //半径0.5以上なら　        disc(st, 0.5) 0    disc(st, 0.4)0     黒　0
                //半径0.5以内なら　        disc(st, 0.5) 1    disc(st, 0.4)0     白　1
                //半径0.4以内なら　        disc(st, 0.5) 1    disc(st, 0.4)1     黒　0
                //を満たせばいい（満たしたい）
				float r1 = disc(st, 0.5) - disc(st, 0.4);//中心からの距離が0.5  を超えない座標なら　　1白  
                                                         //中心からの距離が0.4  を超えない座標なら   1白

                                                            //例 st (uv)の座標が　1,1なら
                                                            //   距離が0.5を超えるので   0
                                                            //   距離が0.495を超えるので 0 結果　0-0で0
                                                            //例 st (uv)の座標が　0.5,0.995なら
                                                            //   距離が0.5を超えないので  1
                                                            //   距離が0.4を超えるので    0 結果　1-0で1  

                float r2 = disc(st, 0.3) - disc(st, 0.2);   
				return r1 + r2 ;//上記の書き方で縞模様になる
			}


            float4 frag (v2f i) : SV_Target
            {                
                return grid(i.uv);
            }
            ENDCG
        }
    }
}
