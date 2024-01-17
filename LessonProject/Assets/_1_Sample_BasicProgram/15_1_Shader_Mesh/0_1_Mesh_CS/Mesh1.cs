using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshSample {
    public class Mesh1 : MonoBehaviour
    {
        [SerializeField] Material material;

        void Start() {
            // 頂点の位置情報
            Vector3[] vertices = {
                new Vector3(-1f, -1f, 0), //配列内の0番目の要素
                new Vector3(-1f,  1f, 0), //配列内の1番目の要素
                new Vector3( 1f,  1f, 0), //配列内の2番目の要素
                new Vector3( 1f, -1f, 0)  //配列内の3番目の要素
            };

            //三角形を作る頂点の順番情報の指示の仕方　右回り正　左手座標系、右回りの回転で法線が決まる
            int[] triangles = { 0, 1, 2, 0, 2, 3 };

            Mesh mesh      = new Mesh();//Meshの入れ物　インスタンス　変数
            mesh.vertices  = vertices;  //頂点　情報　登録
            mesh.triangles = triangles; //三角形情報　登録

            mesh.RecalculateNormals(); //「面の向き」法線計算 (法線ベクトルの情報はmesh.normalsに入ってる)

            MeshFilter meshFilter       = gameObject.GetComponent<MeshFilter>();//ポリゴンの集合体　Mesh情報の登録先
            if (!meshFilter) meshFilter = gameObject.AddComponent<MeshFilter>();//MeshFilterをつける

            MeshRenderer meshRenderer       = gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = gameObject.AddComponent<MeshRenderer>();//MeshRendererをつける

            meshFilter.mesh = mesh;                 //MeshFilterにメッシュを設定
            meshRenderer.sharedMaterial = material; //MeshRendererに表示するマテリアル(材質情報　Shaderから)を設定
        }


    }
}


//Shaderは
//GPU上で動作するプログラムであり、
//描画されるオブジェクトの見た目や振る舞いを定義します。
//これは、オブジェクトがどのように見えるか、どのように反応するかを制御します。

//Materialは、
//Shaderを使用して実際の描画を行うためのプロパティやパラメータを設定するものです。
//Shaderは描画を行うための指示を与えるだけで、
//Materialはその指示に必要な具体的な情報を提供します。
//たとえば、色、テクスチャ、反射率などの情報がMaterialに含まれます。

//MeshFilterは、
//Mesh（メッシュ）と呼ばれる3Dオブジェクトの形状を定義するコンポーネントです。
//メッシュは頂点、法線、UVマップなどのジオメトリ情報を保持します。
//MeshFilterはそのメッシュデータを保持し、
//それを他のコンポーネント（たとえばMeshRenderer）で描画するための情報を提供します。

//MeshRendererは、
//実際にオブジェクトを描画するためのコンポーネントです。
//MeshFilterからメッシュ情報を受け取り、Materialから描画方法を受け取って、オブジェクトを描画します。

//これらのコンポーネントと関連付けられた情報（Mesh、Material、Shaderなど）により、
//Unity内で3Dオブジェクトを作成し、レンダリングすることが可能になります。
//Shaderは描画方法を指定し、Materialは具体的な描画に必要な情報を提供し、MeshFilterは形状情報を提供し、MeshRendererは描画を行います。
