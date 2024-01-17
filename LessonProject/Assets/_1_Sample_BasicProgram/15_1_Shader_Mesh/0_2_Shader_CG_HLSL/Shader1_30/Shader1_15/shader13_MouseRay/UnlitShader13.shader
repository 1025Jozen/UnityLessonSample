Shader "Unlit/UnlitShader13"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : WORLD_POS;
            };
   
            float4 _MousePosition;//C#側から変数の中身が渡される

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);    //いつもの　3D空間座標→スクリーン座標変換              
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);//(unity_ObjectToWorldを使用し)ピクセルのワールド座標を取得
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                
                float4 baseColor = (1,1,1,1);//ベースカラー　白
                
                /*"マウスから出たRayとオブジェクトの衝突箇所(ワールド座標)"
                  と
                  "ピクセルのワールド座標"
                  の距離を求める*/
                float dist = distance( _MousePosition, i.worldPos);
                
                //求めた距離が任意の距離以下なら描画しようとしているピクセルの色を変える
                if( dist < 0.1)
                {                    
                    baseColor *= float4(1,0,0,0);  //赤色乗算代入
                }
                
                return baseColor;
            }
            ENDCG
        }
    }
}