Shader "Unlit/UnlitShader23"
{
    Properties
    {
        //スクロールさせるテクスチャ
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" ="Background" //最背面に描画するのでBackground
            "Queue"      ="Background" //最背面に描画するのでBackground
            "PreviewType"="SkyBox"     //設定すればマテリアルのプレビューがスカイボックスになる
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;//変数の宣言　Propertiesで定義した名前と一致させる

            
            struct appdata    //GPUから頂点シェーダーに渡す構造体
            {
                float4 vertex: POSITION;
            };

            
            struct v2f        //頂点シェーダーからフラグメントシェーダーに渡す構造体
            {
                float4 pos : SV_POSITION;
                float3 worldPos : WORLD_POS;
            };
            
            v2f vert(appdata v)
            {
                v2f o;                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;//頂点のワールド座標
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }


            float4 frag(v2f i) : SV_Target
            {
                
                float3 dir = normalize(i.worldPos);//描画ピクセルのワールド座標 　方向を正規化
                
                
                float2 rad = float2(atan2(dir.x, dir.z), asin(dir.y));
                //ラジアンで算出する　　xz座標　での角度　　　　y座標　　の角度
                
                //tanが角度から底辺と高さの比を表すので その逆であるatanは底辺と高さの比から角度
                //atan2(x,y) 直行座標の角度をラジアンで返す  
                //atan(x)と異なり、1周分の角度をラジアンで返せる　今回はスカイボックスの円周上のラジアンが返される   //asin(x)  -π/2 ～ π/2   の間で逆正弦を返す　xの範囲は -1 ～ 1

                

                //
                float2 uv = rad / float2(2.0 * UNITY_PI, UNITY_PI/2);

                
                float4 col = tex2D(_MainTex, uv);//テクスチャとUV座標から色の計算を行う

                return float4(col);
            }
            ENDCG

        }
    }
}