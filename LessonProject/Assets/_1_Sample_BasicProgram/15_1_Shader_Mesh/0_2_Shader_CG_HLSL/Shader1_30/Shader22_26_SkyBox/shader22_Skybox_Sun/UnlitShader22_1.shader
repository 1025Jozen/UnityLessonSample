Shader "Unlit/UnlitShader22_1"
{
    //y軸方向でグラデーション
    SubShader
    {
        Tags
        {
            "RenderType" ="Background"//最背面に描画するのでBackground
            "Queue"      ="Background"//最背面に描画するのでBackground
            "PreviewType"="SkyBox"    //必須ではないが、設定すればマテリアルのプレビューがスカイボックスになる
        }

        Pass
        {
            ZWrite Off
            //常に最背面に描画するので深度情報の書き込み不要
            // オンにすると各ピクセルがZバッファを保持し、
            // 新たに描画予定のピクセルの深度値との比較が行われます。
            // その比較の結果、深度値が小さいピクセルが前面に表示されます。

            // これはZバッファ法(デプスバッファ法)という手法であり、
            // ピクセル単位で深度情報を保存し、前後関係を正しく描画しやすくする。

            // Skyboxは常に最背面に描画されるものなので、
            // 前後を特定する深度情報は必要ではなく、Queueの描画順序のみで十分。
      
            Cull Off
            //裏面も

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex   : POSITION;
                float3 texcoord : TEXCOORD0;//テクスチャ(Texture) ・座標(Coordinate)の意味 つまりはuv
                //Skyboxシェーダーの特徴は頂点データとして渡されるTEXCOORD0
                // ここには視線方向の三次元の値が[-1, 1]の範囲で入っています。
                // 例えば、デフォルトのカメラは+z軸方向を向いているので、画面の真ん中でのtexcoordの値は[0, 0, 1]になります。
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };


            /*           */
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(i.texcoord * 0.5 + 0.5, 1.0);//+x軸方向が赤 r 、+y軸方向が緑、+z軸方向が青となるようなSkybox

                //色の　割合をuv座標の位置によって補正(y軸の高さによって描画の色が変わる)
                // return fixed4(  lerp(fixed3(1, 0, 0)   ,   fixed3(0, 0, 1)    , i.texcoord.y * 0.5 + 0.5),     1.0);
            }
            ENDCG
        }
    }
}
