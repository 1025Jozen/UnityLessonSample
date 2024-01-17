Shader "Unlit/UnlitShader8"
{
    Properties
    {
        _Color("MainColor",Color) = (0,0,0,0)        
        _SliceSpace("SliceSpace",Range(0,30)) = 15//スライスされる間隔
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

           //変数の宣言　Propertiesで定義した名前と一致させる
            half4 _Color;
            half _SliceSpace;

            struct v2f
            {                
                float4 pos : SV_POSITION;    //posには3D→2D(スクリーン)座標変換された後の頂点座標をいれるよ！ってGPUに教える              
                float3 worldPos : WORLD_POS; //worldPosにはワールド座標をいれるよ！ってGPUに教える
            };

            //すでにUnityCG.cgincで定義されている構造体
            // struct appdata_base {
            //     float4 vertex : POSITION;
            //     float3 normal : NORMAL;
            //     float4 texcoord : TEXCOORD0;
            //     UNITY_VERTEX_INPUT_INSTANCE_ID
            // };

            v2f vert (appdata_base v)
            {
                v2f o;

                //v.vertex各頂点の座標(オブジェクトのローカル座標)をunity_ObjectToWorldを使用し、ワールド座標(xyz成分)に変換しworldPosに保存　
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;//頂点自体に変更は加えてない
            }

            fixed4 frag (v2f i) : SV_Target
            {
                clip(frac(i.worldPos.y * _SliceSpace) - 0.5);
                //頂点処理で取得したワールド座標のy成分に_SliceSpaceを掛けて　frac関数で少数だけ取り出す
        
                // rac関数は引数に渡した値の少数部分のみを返してくれます。
                // 例) frac(1.1) => 0.1

                //そこから-0.5してclip関数に渡す(0を下回ったら描画しない)

                //ワールド座標y軸っ成分　　　　    1   1.01    1.05   1.06  1.09  1.1   1.11  ~
                //frac(_SliceSpace　10の場合)   0   0.1     0.5    0.6   0.9   0     0.1   ~      
                //-0.5 して0を下回ったら　　　　　　0  0        0      0.1   0.4  0  

                //のように一定間隔で描画しないのようなことができる                              

                return half4(_Color);//その上でRGBAを全てのfragmentに当てはめてみる
                //ワールド座標を参照しているのでIbjectを動かすと縞模様もワールド座標に固定されたようになる
            }
            ENDCG
        }
    }
}
