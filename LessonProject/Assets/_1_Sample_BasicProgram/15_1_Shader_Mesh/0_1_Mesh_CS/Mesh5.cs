using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Meshに色を付ける
namespace MeshSample
{
    //全体の流れ
    //頂点の位置情報(vertices)と
    //どの頂点で三角形を作るかという情報(triangles)を配列として用意し
    //それをメッシュに渡す
    public class Mesh5 : MonoBehaviour
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

        [SerializeField] Gradient meshColorGradient;//色のグラデーション設定　ゲージの左端が0%(0.0)、右端が100%(1.0)　　(キーが最大で8個までしか登録できない)

        //地形の最も低い位置と最も高い位置を手動で設定できるように
        [SerializeField] float minHeight;
        [SerializeField] float maxHeight;


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

        //地形の頂点情報を受け取ってテクスチャを返すCreateTextureというメソッドを作成しました。
        //それぞれの頂点のY座標に応じてcolorMap配列に色の情報を格納していきます。
        //Mathf.InverseLerp(a, b, value) は
        //「aを0、bを1とした場合、valueは0～1のどんな値になるか」を調べるメソッドです。
        //たとえばもしMathf.InverseLerp(2, 16, 9)であれば、9は2と16のちょうど中間なので0.5を返します。
        //次の行でその値をmeshColorGradient.Evaluateに渡し、Gradient Editorで設定した色を取得

        Texture2D CreateTexture(Vector3[] vertices) {
            Color[] colorMap = new Color[vertices.Length];//頂点座標分の配列を用意
            for (int i = 0; i < vertices.Length; i++) {
                float percent = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);//頂点配列からy座標を取り出し　高さに合わせたパーセンテージを取得
                colorMap[i] = meshColorGradient.Evaluate(percent);//Gradient Editorで設定した色を取得
            }
            Texture2D texture = new Texture2D(size, size);//テクスチャ用意

            texture.SetPixels(colorMap);//テクスチャに　色を割り当てる
            texture.Apply();

            return texture;
        }
        //void OnValidate() {
        //    CreateMesh();
        //}
    }
}