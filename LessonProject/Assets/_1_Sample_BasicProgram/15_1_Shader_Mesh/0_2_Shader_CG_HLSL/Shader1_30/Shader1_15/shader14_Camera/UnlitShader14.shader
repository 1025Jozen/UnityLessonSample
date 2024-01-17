Shader "Unlit/UnlitShader14"
{
    Properties
    {
        [NoScaleOffset] _NearTex ("NearTexture", 2D) = "white" {}//テクスチャー(オフセットの設定なし)        
        [NoScaleOffset] _FarTex ("FarTexture", 2D)   = "white" {}//テクスチャー(オフセットの設定なし)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }//透明度に関する設定
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _NearTex;
            sampler2D _FarTex;

            struct appdata//頂点シェーダー
            {
                float4 vertex : POSITION; //頂点のローカル座標　モデル内の各頂点の位置情報が格納　この位置情報を変更することで、モデル全体の形状を変更できる
                float2 uv     : TEXCOORD0;//テクスチャ座標     テクスチャ座標は通常、[0,0] から [1,1] の範囲内の値で、テクスチャ上の特定の位置を指定する UV マッピングなどのテクスチャ関連の情報を頂点シェーダー内で使用するために使用
            };

            struct v2f  //フラグメントシェーダーに渡す情報
            {
                float4 vertex   : SV_POSITION;//頂点シェーダーからピクセルシェーダーに出力される頂点のスクリーン座標  各頂点がカメラの視錐台内でのスクリーン座標が格納  各頂点がどの位置に描画されるべきかを制御するのに使用
                float3 worldPos : WORLD_POS;  //ワールド座標系における頂点の位置 各頂点のワールド座標位置を取得しておく
                float2 uv       : TEXCOORD0;  
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv       = v.uv;
                o.vertex   = UnityObjectToClipPos(v.vertex);//
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); //ローカル座標系をワールド座標系に変換して情報を保持
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //それぞれのテクスチャとUVからピクセルの色を取得
                float4 nearCol = tex2D(_NearTex,i.uv);
                float4 farCol  = tex2D(_FarTex ,i.uv);

                
                float cameraToObjLength = length(_WorldSpaceCameraPos - i.worldPos);
                // カメラとオブジェクトの距離(長さ)を取得
                // _WorldSpaceCameraPos：定義済の値　ワールド座標系のカメラの座標
                
                fixed4 col = fixed4(lerp(nearCol, farCol, cameraToObjLength * 0.05));// Lerpを使って色を変化　補間値に"カメラとオブジェクトの距離"を使用
                
                clip(col);//Alphaが0以下なら描画しない
                
                return col;//最終的なピクセルの色を返す
            }
            ENDCG
        }
    }
}
