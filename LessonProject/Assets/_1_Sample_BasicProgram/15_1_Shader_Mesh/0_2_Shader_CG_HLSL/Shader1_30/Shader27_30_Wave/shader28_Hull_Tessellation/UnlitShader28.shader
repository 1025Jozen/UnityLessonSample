Shader "Unlit/UnlitShader28"
{

    Properties
    {
        _Color("Color", color) = (1, 1, 1, 0)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DispTex("Disp Texture", 2D) = "gray" {}
        _MinDist("Min Distance", Range(0.1, 50)) = 10
        _MaxDist("Max Distance", Range(0.1, 50)) = 25
        _TessFactor("Tessellation", Range(1, 50)) = 10 //分割レベル
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
            //頂点シェーダー、フラグメントシェーダーに加えてハルシェーダー



            //頂点  →  ハル  →  テッセレーション  →  Domain  →  geometry  →   Fragment  の順で計算される


            //1 頂点シェーダー (Vertex Shader) 
            //3Dモデルの頂点座標や法線などの情報を計算します。
            //通常、頂点の位置や法線を変換し、ビュー座標変換や投影座標変換を適用します。
            //このステージで計算された頂点情報は、3Dオブジェクトを2D画面上に投影するのに使用されます。

            //2 ハルシェーダー (Hull Shader): 
            //テッセレーションと呼ばれるプロセスの一部として使用されます。
            //テッセレーションは、3Dモデルを細かい三角形に分割し、詳細な表面を生成するプロセスです。
            //ハルシェーダーは、テッセレーション制御やテッセレーション因子を計算するのに使用されます。
            
            //3 テッセレーションシェーダー (Tessellation Shader): 
            //テッセレーションは、細分割された三角形の詳細な表面を生成します。
            //テッセレーションシェーダーは、細分割された三角形の新しい頂点を生成し、新しいポリゴンのトポロジを形成します。
            
            //4 ドメインシェーダー (Domain Shader):
            //テッセレーションによって生成された新しいポリゴンの頂点情報を計算します。
            //このステージで、ポリゴンの頂点座標をローカルスペースからワールドスペースまたはビュースペースに変換できます。

            //5 ジオメトリシェーダー (Geometry Shader):
            //ジオメトリシェーダーは、一連のポリゴンの処理に使用されます。
            //これにより、新しいポリゴンを生成または既存のポリゴンを変更できます。
            //例えば、ジオメトリシェーダーを使用して、テッセレーションされたポリゴンを草のような形状に変更することができます。

            //6 フラグメントシェーダー (Fragment Shader):
            //フラグメントシェーダーはピクセルごとの色情報を計算し、最終的なピクセルの色を生成します。
            //光の影響、テクスチャのサンプリング、シャドウの計算など、3Dオブジェクトの各ピクセルに対する詳細な計算が行われます。
            
            
            CGPROGRAM
            #pragma vertex vert   //vert  が頂点シェーダー       であることをGPUに伝える
            #pragma hull hull     //hull  がハルシェーダー       であることをGPUに伝える
            #pragma domain domain //domainがドメインシェーダー    であることをGPUに伝える
            #pragma fragment frag //frag  がフラグメントシェーダーであることをGPUに伝える

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
            sampler2D _MainTex;
            fixed4 _Color;

            //GPUから頂点シェーダーに渡す構造体
            struct appdata
            {
                float3 vertex   : POSITION;
                float3 normal   : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            //頂点シェーダーからハルシェーダーに渡す構造体
            struct HsInput
            {
                float4 position : POS;
                float3 normal   : NORMAL;
                float2 texCoord : TEXCOORD;
            };

            //ハルシェーダーからテッセレーター経由でドメインシェーダーに渡す構造体
            //テッセレーションを行うと、ポリゴンの分割数を増やしてより滑らかな曲面を作成できたり、
            //分割された頂点をテクスチャを参照して直接盛り上げたり出来ます。
            //ポリゴンを自動で分割してくれる便利なシェーダ　ってこと
            struct HsControlPointOutput
            {
                float3 position : POS;
                float3 normal   : NORMAL;
                float2 texCoord : TEXCOORD;
            };

            //Patch-Constant-Functionからテッセレーター経由でドメインシェーダーに渡す構造体
            struct HsConstantOutput
            {
                float tessFactor[3]    : SV_TessFactor;
                float insideTessFactor : SV_InsideTessFactor;
            };

            //ドメインシェーダーからフラグメントシェーダーに渡す構造体
            struct DsOutput
            {
                float4 position : SV_Position;
                float2 texCoord : TEXCOORD0;
            };


            /*           */


            //「頂点」→ハル→テッセレーション→Domain→geometry→Fragment 
            HsInput vert(appdata i)
            {
                HsInput o;
                o.position = float4(i.vertex, 1.0);
                o.normal = i.normal;
                o.texCoord = i.texcoord;
                return o;
            }

            //頂点  →「ハルシェーダー」  →  テッセレーション  →   Domain   →   geometry   →   Fragment
            
            //ハルシェーダー   どう分割するかを計算してくれます。
            //パッチに対してコントロールポイントを割り当てて出力する   // パッチ ：ポリゴン分割処理を行う際に使用するコントロールポイントの集合
            //コントロールポイントごとに1回実行                    // コントロールポイント：頂点分割で使う制御点
            [domain("tri")]                  //分割に利用する形状を指定　              "tri"     "quad"           "isoline"                        から選択
            [partitioning("integer")]        //分割方法                             "integer" "fractional_eve"  "fractional_odd" "pow2"         から選択
            [outputtopology("triangle_cw")]  //出力された頂点が形成するトポロジー(形状)　"point"   "line"            "triangle_cw"    "triangle_ccw" から選択
            [patchconstantfunc("hullConst")] //Patch-Constant-Functionの指定
            [outputcontrolpoints(OUTPUT_PATCH_SIZE)] //出力されるコントロールポイントの集合の数
            HsControlPointOutput hull(InputPatch<HsInput, INPUT_PATCH_SIZE> i, uint id : SV_OutputControlPointID)
            {
                HsControlPointOutput o = (HsControlPointOutput)0;                
                o.position = i[id].position.xyz;//頂点シェーダーに対してコントロールポイントを割り当て
                o.normal = i[id].normal;
                o.texCoord = i[id].texCoord;
                return o;
            }

            //Patch-Constant-Function
            //どの程度頂点を分割するかを決める係数を詰め込んでテッセレーターに渡す
            //パッチごとに一回実行される
            HsConstantOutput hullConst(InputPatch<HsInput, INPUT_PATCH_SIZE> i)
            {
                HsConstantOutput o = (HsConstantOutput)0;

                float4 p0 = i[0].position;
                float4 p1 = i[1].position;
                float4 p2 = i[2].position;
                //頂点からカメラまでの距離を計算しテッセレーション係数を距離に応じて計算しなおす　LOD的な？
                float4 tessFactor = UnityDistanceBasedTess(p0, p1, p2, _MinDist, _MaxDist, _TessFactor);

                o.tessFactor[0] = tessFactor.x;
                o.tessFactor[1] = tessFactor.y;
                o.tessFactor[2] = tessFactor.z;
                o.insideTessFactor = tessFactor.w;

                return o;
            }


            // 頂点  →   ハル  →   テッセレーション  →  「Domain」  →   geometry   →    Fragment 
            //ドメインシェーダー　　 //テッセレーター(めっちゃポリゴンを分割した)から出てきた分割位置で頂点を計算し出力するのが仕事
            [domain("tri")]      //分割に利用する形状を指定　"tri" "quad" "isoline"から選択
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

                //tex2Dlodはフラグメントシェーダー以外の箇所でもテクスチャをサンプリングできる関数
                //ここでrだけ利用することで波紋の高さに応じて頂点の変位を操作できる！すごい！
                float disp = tex2Dlod(_DispTex, float4(o.texCoord, 0, 0)).r * _Displacement;
                f3Position.xyz += f3Normal * disp;

                o.position = UnityObjectToClipPos(float4(f3Position.xyz, 1.0));

                return o;
            }

            //「頂点」→「ハル」→テッセレーション→Domain→geometry→「Fragment」 
            fixed4 frag(DsOutput i) : SV_Target
            {
                return tex2D(_MainTex, i.texCoord) * _Color;
            }
            ENDCG
        }
    }

    Fallback "Unlit/Texture"

}