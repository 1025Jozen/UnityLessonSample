Shader "Custom/UnlitShader32"
{
    Properties
    {
        _Color1("Color 1",Color) = (0,0,0,1)
        _Color2("Color 2",Color) = (1,1,1,1)
    }

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
            #pragma fragment frag
            #include "UnityCG.cginc"

            //頂点シェーダーに渡ってくる頂点データ
            struct appdata
            {
                float4 vertex : POSITION;
            };

            //フラグメントシェーダーへ渡すデータ
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : WORLD_POS;
            };

            float4 _Color1;
            float4 _Color2;

            //頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;//ピクセルのワールド座標
                o.vertex = UnityObjectToClipPos(v.vertex);          //3D空間座標→スクリーン座標変換
                return o;
            }

            //フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                float dotResult = dot(i.worldPos,normalize(float2(1,1)));//　ワールド座標のベクトル　　 斜めのベクトルと内積
                float repeat    = abs(dotResult - _Time.w);    //内積を 時間で引く
                float interpolation = step(fmod(repeat,1),0.1);//fmodで小数が返る　周期的に数値が得られる　　　　stepで　0.1以下なら　1が返る　0~0.1で1 0.1~0.99 で0　のような周期
                //fmod(a,b)はaをbで除算した正の剰余が得られる。a % b
                //つまり、余りが返ってくるのですが、浮動小数点数の余りを返してくれる

                // step(a,b)はbがaより大きい場合は1を返す a < b なら 1

                fixed4 col      = lerp(_Color1, _Color2, interpolation);// 斜め方向に　1:9の割合で色が変わって　色が動く
                return col;
            }
            ENDCG
        }
    }
}