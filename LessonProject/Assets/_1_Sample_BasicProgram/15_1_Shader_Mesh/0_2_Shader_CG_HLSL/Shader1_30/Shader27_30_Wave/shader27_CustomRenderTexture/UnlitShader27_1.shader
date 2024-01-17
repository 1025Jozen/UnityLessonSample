Shader "Unlit/UnlitShader27_1"      //Custom Renderer Textureに関するShader
{
    //CustomできるRenderTexture 通常
    //カメラの描画結果を映してサブモニターにしたり、
    //動画を貼り付けてスクリーンとして使ったりできる。
    //CustomRenderTextureでサブテクスチャーのように使用してみてる

    Properties
    {
        _S2("PhaseVelocity^2", Range(0.0, 0.5)) = 0.2
        [PowerSlider(0.01)]
        _Attenuation("Attenuation", Range(0.0, 1.0)) = 0.999
        _DeltaUV("Delta UV", Float) = 3
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc" //専用のcgincファイル
            
            #pragma vertex   CustomRenderTextureVertexShader //専用の定義済みvertexシェーダ関数
            #pragma fragment frag

       
            //Propertiesと一緒
            half _S2;         //速度
            half _Attenuation;//減衰率
            float _DeltaUV;   //

            sampler2D _MainTex;//



            //色に関して
            float4 frag(v2f_customrendertexture i) : SV_Target
            {
                float2 uv = i.globalTexcoord;//カスタムレンダーテクスチャそのものに関連するテクスチャ座標(customに関するuvとして)

                // 1pxあたりの大きさを計算する
                float du = 1.0 / _CustomRenderTextureWidth; //横　　　unityで用意されてる変数 CustomRenderTextureWidthの横幅
                float dv = 1.0 / _CustomRenderTextureHeight;//たて　　unityで用意されてる変数 CustomRenderTextureWidthの縦幅
                float2 duv = float2(du, dv) * _DeltaUV;     //波の次の瞬間のuv位置　

                // 現在の位置のテクセルをフェッチ  unityで用意されてる変数 _SelfTexture2D 最後の更新結果を含むテクスチャ　
                // tex2D関数は、UV座標(uv)からテクスチャ上のピクセルの色を計算して返します。
                //テクセル テクスチャ空間の基本単位
                float4 c = tex2D(_SelfTexture2D, uv);//最終更新位置での色

                //2階微分　＝  変化量　の(における)　変化具合　　(2階微分はモノを表して、2回微分は動作を表す感じ)
                //例：u(t,x)と時刻に関してと場所に関して考えられるか１つを固定して微小な変化を考える(偏微分)
                // 時刻 t が n-1 → n → n+1 における   変化をプログラム的に表すなら       
                //(u[n+1] - u[n]) - (u[n] - u[n-1]) = u[n+1] + u[n-1] - 2 * u[n]

                //波動方程式  u(t+1) 次の瞬間の波の高さを求める

                //tに関しての２階微分 　場所固定  次の時間の様子は？ //xとyに関しての2 階微分　　時間固定　次の場所はどんな様子
                //(u(t+1)-u(t))- (u(t)-u(t-1)) = c*c*( (u(x+1) - u(x)) - (u(x) - u(x-1) )  +  (u(y+1) - u(y)) - (u(y) - u(y-1) ) )
                //u(t+1)-2u(t)+u(t-1)          = c*c*( u(x+1)-2u(x)+u(x-1) + u(y+1)-2u(y)+u(y-1) )
                //u(t+1)                       = 2u(t) - u (t-1) + c*c*( u(x+1)+u(x-1) + u(y+1) + u(y-1) -2u(x) -2u(y) )　　 
                //u(x)とu(y)は同じこと

                //今回はテクスチャの赤要素を波として捉える

                //今回、u(t + 1)は次のフレームでの波の高さを表す　　u(t)が現在　u(t-1)が前のフレーム

                //場所固定したとき(１ピクセルのfragの色を変えるとき)
                
                //R,G,B  (float2 の2成分  cをfloat4出せば) をそれぞれ高さとして使用
                          //2u(t) - u (t-1)  テクスチャのR成分を現在の高さ(数値)　テクスチャのG成分を前の高さ(数値)として利用してみる Gを利用したのは無理やり(波の)動きを出させるため   ( b要素 0なので)
                float p = ((2.0 * c.r) - c.g
                             + _S2 * ( //_S2は位相の変化する速度 波動方程式では2乗されてるが、ここでは２乗したていで
                                        tex2D(_SelfTexture2D, uv + duv.x).r +//現在のuv位置でのテクスチャの色 　その隣(xプラス方向)のRの数値
                                        tex2D(_SelfTexture2D, uv - duv.x).r +//現在のuv位置でのテクスチャの色 　その隣(xマイナス方向)のRの数値
                                        tex2D(_SelfTexture2D, uv + duv.y).r +//現在のuv位置でのテクスチャの色 　その隣(yプラス方向)のRの数値
                                        tex2D(_SelfTexture2D, uv - duv.y).r  //現在のuv位置でのテクスチャの色 　その隣(yマイナス方向)のRの数値
                                        - 4 * tex2D(_SelfTexture2D, uv).r    //現在のuv位置でのテクスチャの色 　のRの数値
                                    )
                        ) * _Attenuation; //減衰係数
                        //次のr成分の数値をp 現在のテクスチャのr成分をG成分に　上で前の数値の保存場所としてG成分位置を使用したので
                return float4(p, c.r, 0, 0);
            }
            ENDCG
        }
    }
}