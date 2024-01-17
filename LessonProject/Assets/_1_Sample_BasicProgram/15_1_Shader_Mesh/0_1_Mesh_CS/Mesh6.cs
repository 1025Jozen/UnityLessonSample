using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//頂点数がどんどん増えていくので　専用のシェーダーを使った方がいいかも
namespace MeshSample
{
    //全体の流れ
    //頂点の位置情報(vertices)と
    //どの頂点で三角形を作るかという情報(triangles)を配列として用意し
    //それをメッシュに渡す
    public class Mesh6 : MonoBehaviour
    {
        [Range(2, 255)]
        [SerializeField] int size;
        [SerializeField] float vertexDistance = 1f;//頂点間の距離
        [SerializeField] Material material;
        [SerializeField] PhysicMaterial physicMaterial;

        [SerializeField] PerlinNoiseProperty[] perlinNoiseProperty = new PerlinNoiseProperty[1];
        [System.Serializable]
        public class PerlinNoiseProperty
        {
            public float heightMultiplier = 1f;
            public float scale = 1f;
            public Vector2 offset;
        }

        [SerializeField] Gradient meshColorGradient;

        [SerializeField] float minHeight;
        [SerializeField] float maxHeight;

        [SerializeField] bool blockMode;
        [Range(1, 16)]
        [SerializeField] int textureDetail;

        void Start() {
            CreateMesh();
        }

        void CreateMesh() {
            Vector3[] vertices = new Vector3[size * size];
            for (int z = 0; z < size; z++) {
                for (int x = 0; x < size; x++) {

                    float sampleX;
                    float sampleZ;
                    float y = 0;
                    foreach (PerlinNoiseProperty p in perlinNoiseProperty) {//後から付け足したプロパティをy座標情報とし加算
                        p.scale = Mathf.Max(0.0001f, p.scale);
                        sampleX = (x + p.offset.x) / p.scale;
                        sampleZ = (z + p.offset.y) / p.scale;
                        y += Mathf.PerlinNoise(sampleX, sampleZ) * p.heightMultiplier;//PerlinNoise = 擬似ランダムパターン(白黒モザイクのやつ)　座標からPerlinNoiseの値取得
                    }

                    vertices[z * size + x] = new Vector3(x * vertexDistance, y, -z * vertexDistance);
                }
            }

            int triangleIndex = 0;
            int[] triangles = new int[(size - 1) * (size - 1) * 6];
            for (int z = 0; z < size - 1; z++) {
                for (int x = 0; x < size - 1; x++) {

                    int a = z * size + x;
                    int b = a + 1;
                    int c = a + size;
                    int d = c + 1;

                    triangles[triangleIndex] = a;
                    triangles[triangleIndex + 1] = b;
                    triangles[triangleIndex + 2] = c;

                    triangles[triangleIndex + 3] = c;
                    triangles[triangleIndex + 4] = b;
                    triangles[triangleIndex + 5] = d;

                    triangleIndex += 6;
                }
            }


            //テクスチャをメッシュのどの部分に対応させるかを示すもの
            //UVの配列数はメッシュの頂点数と同じ

            //テクスチャの左下部分が(0, 0)、テクスチャの中心が(0.5, 0.5)、テクスチャの右上が(1, 1)とそれぞれ対応
            Vector2[] uvs = new Vector2[size * size];
            for (int z = 0; z < size; z++) {
                for (int x = 0; x < size; x++) {
                    uvs[z * size + x] = new Vector2(x / (float)size, z / (float)size);//サイズが3の場合　(0/3,0/3)   (1/3,0/3)  (2/3,0/3)・・・
                }
            }




            Mesh mesh      = new Mesh();
            mesh.vertices  = vertices;
            mesh.triangles = triangles;
            mesh.uv        = uvs;      //テクスチャのためのUV配列

            mesh.RecalculateNormals();

            MeshFilter meshFilter       = gameObject.GetComponent<MeshFilter>();
            if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

            MeshRenderer meshRenderer       = gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

            MeshCollider meshCollider       = gameObject.GetComponent<MeshCollider>();
            if (!meshCollider) meshCollider = gameObject.AddComponent<MeshCollider>();

            meshFilter.mesh = mesh;
            meshRenderer.sharedMaterial = material;
            meshRenderer.sharedMaterial.mainTexture = CreateTexture(vertices);//MeshRendererにテクスチャを設定　　


            meshCollider.sharedMesh     = mesh;
            meshCollider.sharedMaterial = physicMaterial;
  
        }


        Texture2D CreateTexture(Vector3[] vertices) {
            //解像度をあげてみる
            //textureDetailの値が大きくなるごとにメッシュの頂点を補間する数を多くします。　
            //メッシュの中に　textureDetailという頂点を増やす感じ. 辺2分割するイメージ
            //サイズが3　（頂点数は3*3)　　meshが4つ　で
            //textureDetailが２の場合
            //
            int textureSize = (size - 1) * textureDetail + 1;//増やした後の頂点数 //通常のメッシュの数が2*2 １つのmeshを２分割する場合　辺が４分割　頂点数を数えるならプラス１


            Color[] colorMap = new Color[textureSize * textureSize];//全ての頂点数分の配列を用意　
            for (int z = 0; z < textureSize; z++) {
                for (int x = 0; x < textureSize; x++) {
                    float sampleX;
                    float sampleZ;
                    float y = 0;
                    //それぞれの点でのY座標を求めます
                    foreach (PerlinNoiseProperty p in perlinNoiseProperty) {
                        p.scale = Mathf.Max(0.0001f, p.scale);
                        sampleX = (x / (float)textureDetail + p.offset.x) / p.scale;
                        sampleZ = (z / (float)textureDetail + p.offset.y) / p.scale;
                        y += Mathf.PerlinNoise(sampleX, sampleZ) * p.heightMultiplier;
                    }

                    float percent = Mathf.InverseLerp(minHeight, maxHeight, y);
                    colorMap[z * textureSize + x] = meshColorGradient.Evaluate(percent);//配列の要素として色の割合を保存
                }
            }
            Texture2D texture = new Texture2D(textureSize, textureSize);//テクスチャの要素　１辺の頂点数　


            texture.SetPixels(colorMap);//配列を割り当てる　　[１つ目の要素の頂点の色の割合 ,２つ目の要素の頂点の色の割合 ,３つ目の要素の頂点の色の割合  ,・・・]  
            texture.wrapMode = TextureWrapMode.Clamp; //テクスチャの貼り方を選択。
                                                      //Repeatに設定するとテクスチャを繰り返して隙間なく埋める、
                                                      //Clampにするとテクスチャを引き伸ばしてメッシュの端にピッタリ合わせて貼る
            if (blockMode) texture.filterMode = FilterMode.Point;//テクスチャを3Dのモデルに貼り付けるときにどのように拡大するか
                                                                 //Pointにするとそれぞれのピクセルがブロック状に
            texture.Apply();

            return texture;
        }
        //void OnValidate() {
        //    CreateMesh();
        //}
    }
}

//このコードはさすがにパフォーマンス的に無理があります。
//たとえばメッシュのsizeが64の場合、実際の頂点数は64×64=4096個。
//textureDetailを4に設定した場合、テクスチャのサンプル数は253×253で64009個にもなります。
//textureDetailが増えるごとにそのおよそ2乗分が増えていくことになるので、
//そのすべてでパーリンノイズを算出して処理していくのはとんでもなくコストがかかります。
//指定したタイミングで1度だけ読み込むとかならいいと思いますが、
//たとえば高解像度のテクスチャを毎フレーム計算したりするのは現実的ではありません。
//というわけで、さすがにもうテクスチャを貼り付けるだけでは間に合わなくなってきました。
//ここまで作っておいてなんですが、ここまでくるとおとなしく専用のシェーダーを用意したほうがよさそうですね。