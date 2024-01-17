Shader "Custom/UnlitShader31"
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

            /*       */
            //頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;//ピクセルのワールド座標　　　//unity_ObjectToWorld × 頂点座標(v.vertex)
                o.vertex = UnityObjectToClipPos(v.vertex);          //3D空間座標→スクリーン座標変換
                return o;
            }

            //フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {              
                float interpolation = dot(i.worldPos,float2(0,1));            //各ピクセルのワールド座標の位置ベクトルを正規化していないパターン
                
                //float interpolation = dot(i.worldPos,normalize(float2(1,1))));//斜め方向のベクトルを利用
                	        
                // float interpolation = dot(normalize(i.worldPos),float2(0,1));//　ワールド座標での位置と (0,1)ベクトルの　　内積結果　 ||・||cos　-1~1 //単位ベクトル同士　
                
                //内積　
                fixed4 col = lerp(_Color1, _Color2, interpolation);//

                return col;//1白
            }
            ENDCG
        }
    }
}
