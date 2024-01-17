Shader "test/DrawTracks"//絵を描く　//この絵を元に凸凹を考える
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Coordinate ("Coordinate",Vector) = (0,0,0,0)//　跡
        _Color ("Draw Color" , Color) = (1,0,0,0)    //
        _Size("Size",Range(1,500)) = 50
        _Strength("Strength",Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            ///

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Coordinate , _Color;
            float _Size;
            float _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);//テクスチャのUV座標にならった形で色を把握

                //押した位置を描く
                //uv座標  と　押した位置の距離　を出す
                //1 1    と　　　マウスを押した位置でのUV座標
                //powで大きさ調整　
                float draw = pow( saturate( 1 - distance(i.uv,_Coordinate.xy)), _Size);//　距離　　Saturateは0以下の値を0に、1以上の値を1   
                fixed4 drawcol = _Color * ( draw * 500/_Strength);
                return saturate(col + drawcol);



                //テスト   場所に色を塗る
                //float radius = 0.5f;
                //float dist = distance(i.uv, float2(_Coordinate.x, _Coordinate.y));// 描画位置との 距離
                //float alpha = 1.0 - smoothstep(radius - 0.01, radius + 0.01, dist);//
                //return _Color * alpha;


                
            }
            ENDCG
        }
    }
}
