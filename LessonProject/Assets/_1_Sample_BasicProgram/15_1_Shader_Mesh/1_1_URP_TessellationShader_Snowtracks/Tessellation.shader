//全体の流れ
//Tessellation.shader
//第1段階　テッセレーション　でポリゴン数を増やす
//第2段階　既存の絵(テクスチャ)の色合い(R)を元に頂点を上下させる
//第3段階　深さを元に２つのテクスチャや色合いを混ぜる(影であったり地面であってり)

//DrawTracks.shader DrawTracks.cs
//第4段階　マウスでクリックした位置にRayを飛ばし、そのRayを元にtempのテクスチャに絵を描き、そのテクスチャをTessellation.shaderで参照してもらう

//ObjectTracks.cs
//第5段階  マウスの代わりにObjectからRayを飛ばし、そのRayを元にtempのテクスチャに絵を描き、そのテクスチャをTessellation.shaderで参照してもらう

//SnowFall.shader  SnowNoise.cs 
//第6段階  時間経過によって凸凹を平にする、SnowNoise.csはプログラムからSnowFall.shaderやTessellation.shaderの変数を操作できるようにしただけのもの
//        時間経過によって、参照してもらう絵の色合いを変える

Shader "test/Tessellation"
{

    Properties
    {
        _SnowColor  ("SnowColor", color) = (1, 1, 1, 0)
        _SnowTex("SnowBase (RGB)", 2D)   = "white" {}   //見た目
        _GroundColor("GroundColor", color) = (1, 1, 1, 0) //雪を踏んだら影なり地面なりが見えるはず
        _GroundTex("GroundBase (RGB)", 2D)   = "white" {}

        _DispTex("Disp Texture", 2D) = "gray" {}     //凸凹を表現するときに使うテクスチャ
        _MinDist("Min Distance", Range(0.1, 50)) = 10
        _MaxDist("Max Distance", Range(0.1, 50)) = 25
        _TessFactor  ("Tessellation", Range(1, 50))  = 10  //分割レベル
        _Displacement("Displacement", Range(0, 1.0)) = 0.3 //変位
    }

    SubShader
    {

        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            
	//      項目                     	概要
    //1.	VS (頂点シェーダー ステージ)	頂点ごとの処理を実装
    //2.	HS (ハルシェーダー ステージ)	テッセレータ用のパラメータの設定を行う
    //3.	TS (テッセレータ ステージ)	    メッシュを分割する (カスタマイズ不可)
    //4.	DS (ドメインシェーダー ステージ)	分割後のメッシュの頂点計算を行う (MVP座標変換など)
    //5.	PS (ピクセルシェーダー ステージ)	色の計算を行う (テクスチャサンプリングなど)
            CGPROGRAM
            #pragma vertex vert   // vertが頂点シェーダーであることをGPUに伝える
            #pragma fragment frag // fragがフラグメントシェーダーであることをGPUに伝える
            #pragma hull hull     // hullがハルシェーダーであることをGPUに伝える
            #pragma domain domain // domainがドメインシェーダーであることをGPUに伝える

            #include "Tessellation.cginc"
            #include "UnityCG.cginc"

            //定数を定義
            #define INPUT_PATCH_SIZE 3
            #define OUTPUT_PATCH_SIZE 3

            float _TessFactor;
            float _Displacement;
            float _MinDist;
            float _MaxDist;
            sampler2D _DispTex;
            sampler2D _SnowTex;
            sampler2D _GroundTex;
            fixed4 _SnowColor;
            fixed4 _GroundColor;

   
	//      項目                     	概要
    //1.	VS (頂点シェーダー ステージ)	頂点ごとの処理を実装
    //2.	HS (ハルシェーダー ステージ)	テッセレータ用のパラメータの設定を行う
    //3.	TS (テッセレータ ステージ)	    メッシュを分割する (カスタマイズ不可)
    //4.	DS (ドメインシェーダー ステージ)	分割後のメッシュの頂点計算を行う (MVP座標変換など)
    //5.	PS (ピクセルシェーダー ステージ)	色の計算を行う (テクスチャサンプリングなど)         

            //0→1
            struct appdata//GPUから頂点シェーダーに渡す構造体
            {
                float3 vertex   : POSITION;//メッシュの頂点の位置座標。4次元ベクトルであるが、4つ目の値は演算上の都合で存在。常に1が入る。
                float3 normal   : NORMAL;  //頂点の法線
                float2 texcoord : TEXCOORD0;//1番目のUV座標 Texture Coordinateの略で、(Coordinate:座標) メッシュ情報の中のUV座標という情報に対応する。
            };

            //1→2
            struct HsInput//頂点シェーダーからハルシェーダーに渡す構造体
            {
                float4 position : POS;
                float3 normal   : NORMAL;
                float2 texCoord : TEXCOORD;
            };

            //2→3→4 ハルシェーダーからテッセレーター経由でドメインシェーダーに渡す構造体
            struct HsControlPointOutput
            {
                float3 position : POS;
                float3 normal   : NORMAL;
                float2 texCoord : TEXCOORD;
            };

            //2→3→4 Patch-Constant-Functionからテッセレーター経由でドメインシェーダーに渡す構造体
            struct HsConstantOutput
            {
                float tessFactor[3]    : SV_TessFactor;
                float insideTessFactor : SV_InsideTessFactor;
            };

            //4→5 ドメインシェーダーからフラグメントシェーダーに渡す構造体
            struct DsOutput
            {
                float4 position : SV_Position;//フラグメントシェーダに渡すScreen空間に変換されたメッシュの頂点の位置座標。
                float2 texCoord : TEXCOORD0;//texCoord　に応じて　凸凹をつける　色をつける

            };


	//      項目                     	概要
    //1.	VS (頂点シェーダー ステージ)	頂点ごとの処理を実装
    //2.	HS (ハルシェーダー ステージ)    2-1.頂点分割で使う制御点(コントロールポイント)の出力 2-2.パッチ定数(ポリゴン分割処理を行う際に使用するコントロールポイントの集合)の出力
    //3.	TS (テッセレータ ステージ)	    メッシュを分割する (カスタマイズ不可)
    //4.	DS (ドメインシェーダー ステージ)	分割後のメッシュの頂点計算を行う (MVP座標変換など)
    //5.	PS (ピクセルシェーダー ステージ)	色の計算を行う (テクスチャサンプリングなど)

    //1.頂点シェーダー
            HsInput vert(appdata i)
            {
                HsInput o;
                o.position = float4(i.vertex, 1.0);
                o.normal   = i.normal;
                o.texCoord = i.texcoord;
                return o;
            }

    //2.ハルシェーダー
    //2-1 頂点分割で使う制御点(コントロールポイント)の出力
            //パッチに対してコントロールポイントを割り当てて出力する
            //コントロールポイントごとに1回実行
            [domain("tri")] //分割に利用する形状を指定　"tri" "quad" "isoline"から選択
            [partitioning("integer")] //分割方法 "integer" "fractional_eve" "fractional_odd" "pow2"から選択
            [outputtopology("triangle_cw")] //出力された頂点が形成するトポロジー(形状)　"point" "line" "triangle_cw" "triangle_ccw" から選択
            [patchconstantfunc("hullConst")] //Patch-Constant-Functionの指定
            [outputcontrolpoints(OUTPUT_PATCH_SIZE)] //出力されるコントロールポイントの集合の数
            HsControlPointOutput hull(InputPatch<HsInput, INPUT_PATCH_SIZE> i, uint id : SV_OutputControlPointID)
            {
                HsControlPointOutput o = (HsControlPointOutput)0;

                //頂点シェーダーに対してコントロールポイントを割り当て
                o.position = i[id].position.xyz;
                o.normal   = i[id].normal;
                o.texCoord = i[id].texCoord;
                return o;
            }

    //2-2
            //Patch-Constant-Function
            //どの程度頂点を分割するかを決める係数を詰め込んでテッセレーターに渡す
            //パッチごとに一回実行される
            HsConstantOutput hullConst(InputPatch<HsInput, INPUT_PATCH_SIZE> i)
            {
                HsConstantOutput o = (HsConstantOutput)0;

                float4 p0 = i[0].position;
                float4 p1 = i[1].position;
                float4 p2 = i[2].position;

                //頂点からカメラまでの距離を計算しテッセレーション係数を距離に応じて計算しなおす　LOD(閾値)的な 　遠かったら頂点数を減らす
                float4 tessFactor = UnityDistanceBasedTess(p0, p1, p2, _MinDist, _MaxDist, _TessFactor);

                o.tessFactor[0] = tessFactor.x;
                o.tessFactor[1] = tessFactor.y;
                o.tessFactor[2] = tessFactor.z;

                o.insideTessFactor = tessFactor.w;

                return o;
            }


            //(テッセレータの入力は自動)
    //3. TS (テッセレータ ステージ)
            // ハルシェーダーで作成した制御点とパッチ定数を元に、メッシュが分割されます。
            // テッセレータの処理を自分で書くことはできません。


    //4. ドメインシェーダー
            //テッセレーターから出てきた分割位置で頂点を計算し出力するのが仕事
            //  ドメインシェーダーには、
            //     パッチ定数 HsConstantOutput と、
            //     三角形を構成する制御点 HsControlPointOutput の配列(サイズ=3)、
            //     ドメインポイントの位置 baryが入力されます。 bary は重心座標になっています。

            //ドメインポイント bary(重心座標) を利用して、
            //三角形の各頂点 input の座標の加重平均を計算すると、
            //ドメインポイントの座標が求まります。
            // ドメインポイントの座標を計算 (三角形の頂点の座標Positionの加重平均)

            [domain("tri")] //分割に利用する形状を指定　"tri" "quad" "isoline"から選択
            DsOutput domain(
                HsConstantOutput hsConst,
                const OutputPatch<HsControlPointOutput, INPUT_PATCH_SIZE> i,
                float3 bary : SV_DomainLocation)
            {
                DsOutput o = (DsOutput)0;

                //新しく出力する各頂点の座標を計算
                float3 f3Position =
                    bary.x * i[0].position +
                    bary.y * i[1].position +
                    bary.z * i[2].position;

                //新しく出力する各頂点の法線を計算
                float3 f3Normal = normalize(
                    bary.x * i[0].normal +
                    bary.y * i[1].normal +
                    bary.z * i[2].normal);

                //新しく出力する各頂点のUV座標を計算
                o.texCoord =
                    bary.x * i[0].texCoord +
                    bary.y * i[1].texCoord +
                    bary.z * i[2].texCoord;

                //_DispTexはcsファイルで　別のテクスチャ _splatmap に変えた　そのテクスチャをもとに凹ませる

                //tex2Dlod  フラグメントシェーダー以外の箇所で、テクスチャをサンプリングできる関数
                //テクスチャの色( tex2Dlod(_DispTex, float4(o.texCoord, 0, 0)).r )　に応じて頂点の変位を操作　　texCoordのuvに応じて、_DispTexを参照した凸凹をつける       0~1 に今はしてる Displacement
                float disp = tex2Dlod(_DispTex, float4(o.texCoord, 0, 0)).r * _Displacement;
                //f3Position.xyz += f3Normal * disp;//各頂点の座標f3Position.xyzに　法線f3Normal * 変位 を足して凹凸をつける
                f3Position.xyz -= f3Normal * disp;

                o.position = UnityObjectToClipPos(float4(f3Position.xyz, 1.0));//3d座標を2dに変換

                return o;
            }

    //5. PS (ピクセルシェーダー フラグメントシェーダー ステージ)
            //   ドメインシェーダーの出力データを利用して、色の計算処理を実装します。
            fixed4 frag( DsOutput i) : SV_Target//出力する画面上の色。RGBAを指定する
            {
                //雪の色
                fixed4 c_snow = tex2D(_SnowTex, i.texCoord) * _SnowColor;

                //地面・影の色 i.texCoord　uv座標によって色合いを取得
                fixed4 c_ground = tex2D(_GroundTex, i.texCoord) * _GroundColor;


                //lerpの割合 を決める　深さ（テクスチャの色合いから深さを決めてる）で量を変える　
                half amount =  tex2Dlod(_DispTex, float4(i.texCoord, 0, 0)).r ;

                //lerpで深さによって色をゆっくり変える
                return lerp(  c_snow, c_ground , amount );
            }
            ENDCG

        }
    }
    Fallback "Unlit/Texture"

}