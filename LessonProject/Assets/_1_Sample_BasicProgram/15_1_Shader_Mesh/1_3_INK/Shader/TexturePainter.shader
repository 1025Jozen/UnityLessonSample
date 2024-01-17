Shader "TNTC/TexturePainter"{   

    Properties{
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
    }

    SubShader{
        Cull Off ZWrite Off ZTest Off

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float3 _PainterPosition;//塗る位置　レイが判定した位置
            float  _Radius;
            float  _Hardness;
            float  _Strength;
            float4 _PainterColor;   //
            float  _PrepareUV;      //

            struct appdata{
                float4 vertex : POSITION; //任意の座標情報
				float2 uv     : TEXCOORD0;//UV座標
            };

            struct v2f{
                float4 vertex   : SV_POSITION;//任意ではない座標情報
                float2 uv       : TEXCOORD0;  //UV座標
                float4 worldPos : TEXCOORD1;  //UV座標
            };


            //関数　　距離によって　返す値・数字(0~1)を返し　色付けの判断に使う
            float mask(float3 position, float3 center, float radius, float hardness){
                float m = distance(center, position);               //centerとUV座標との距離を取得
                return 1 - smoothstep(radius * hardness, radius, m);//距離によって 0~1の値を返す
                //軌跡の大きさを計算 smoothstep(a,b,c) は cがa以下の時は0、b以上の時は1、  0～1は補間
                //1 - smoothstep(a,b,c)とすることで補間値を逆転できる　
                //つまり      1 - smoothstep(a,b,c) は  cがa以上の時は1、b以下の時は0、0～1は補間
            }

            //頂点
            v2f vert (appdata v){
                v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);//ワールド空間の位置をuv空間に変換する　uvアイランドを再作成　UVに重複する三角形がないことを意味する
                o.uv       = v.uv;
				float4 uv  = float4(0, 0, 0, 1);
                uv.xy      = float2(1, _ProjectionParams.x) * (v.uv.xy * float2( 2, 2 ) - float2(1, 1));//レンダリング投影行列　_ProjectionParams.x　＊  -1~1
                //uv.xy      = (v.uv.xy * 2 - 1) * float2(1, _ProjectionParams.x) ;//_ProjectionParams x は 1.0 または –1.0、反転した射影行列で現在レンダリングしている場合は負の値。
                o.vertex   = uv; 
                return o;
            }

            //フラグメント
            fixed4 frag (v2f i) : SV_Target{   
                if(_PrepareUV > 0 ){
                    return float4(0, 0, 1, 1);
                }         

                float4 col = tex2D(_MainTex, i.uv);
                float f    = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);//0~1
                float edge = f * _Strength;
                return lerp(col, _PainterColor, edge);//背景色とブラシの色の間を補間
            }
            ENDCG
        }
    }
}