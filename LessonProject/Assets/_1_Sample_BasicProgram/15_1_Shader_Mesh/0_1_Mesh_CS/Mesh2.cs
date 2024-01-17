using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MeshSample
{
    //全体の流れ
    //頂点の位置情報(vertices)と
    //どの頂点で三角形を作るかという情報(triangles)を配列として用意し
    //それをメッシュに渡す
    public class Mesh2 : MonoBehaviour
    {
        [Range(2, 255)]
        [SerializeField] int size;//全体の頂点数 4なら4×4で16個 (1つのメッシュが持てる頂点数の限界65534個)
        [SerializeField] float vertexDistance = 1f;//頂点間の距離
        [SerializeField] Material material;
        [SerializeField] PhysicMaterial physicMaterial;

        void Start() {

            //サイズから配列用意
            Vector3[] vertices = new Vector3[size * size];
            //3の場合下のような位置情報を配列として持つ
            //012
            //345
            //678
            for (int z = 0; z < size; z++) {
                for (int x = 0; x < size; x++) {
                    vertices[z * size + x] = new Vector3(x * vertexDistance, 0, -z * vertexDistance);
                }
            }

            int triangleIndex = 0;
            int[] triangles = new int[(size - 1) * (size - 1) * 6];//すべての三角形の頂点数 2の場合6 　3の場合 24
            //頂点番号　　３つずつ　見て　meshが生成される
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

            Mesh mesh = new Mesh();
            mesh.vertices  = vertices; //meshに頂点情報(配列)
            mesh.triangles = triangles;//meshに三角形情報(配列)

            mesh.RecalculateNormals();//面

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();

            MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
            if (!meshCollider) meshCollider = gameObject.AddComponent<MeshCollider>();

            meshFilter.mesh             = mesh;
            meshRenderer.sharedMaterial = material;

            meshCollider.sharedMesh     = mesh;
            meshCollider.sharedMaterial = physicMaterial;
        }
    }
}