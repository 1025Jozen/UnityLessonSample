using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//パーリンノイズ
namespace MeshSample
{
    //全体の流れ
    //頂点の位置情報(vertices)と
    //どの頂点で三角形を作るかという情報(triangles)を配列として用意し
    //それをメッシュに渡す
    public class Mesh4 : MonoBehaviour
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

            Mesh mesh      = new Mesh();
            mesh.vertices  = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();

            MeshFilter meshFilter       = gameObject.GetComponent<MeshFilter>();
            if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

            MeshRenderer meshRenderer       = gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

            MeshCollider meshCollider       = gameObject.GetComponent<MeshCollider>();
            if (!meshCollider) meshCollider = gameObject.AddComponent<MeshCollider>();

            meshFilter.mesh = mesh;
            meshRenderer.sharedMaterial = material;
            meshCollider.sharedMesh     = mesh;
            meshCollider.sharedMaterial = physicMaterial;
        }

        //インスペクタ上で数値が変更されるごとに自動で呼び出されるメソッドです。
        //size変数をあまり大きくしすぎると処理に時間がかかってしまうので、ほどほどに
        //void OnValidate() {
        //    CreateMesh();
        //}
    }
}