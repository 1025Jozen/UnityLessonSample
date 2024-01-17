using System.Collections.Generic;
using UnityEngine;


namespace Shader_Sample
{
    /// <summary> メッシュをランタイムで生成する </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshGenerate : MonoBehaviour
    {
       [SerializeField] Material mat;

        private void Start() {
            FourSidedPyramid();
        }

        /// <summary> 四角錐を生成 </summary>
        private void FourSidedPyramid() {
            //四角錐の頂点を作成する
            var vertices = new List<Vector3>() {
                // 0,1,2,3
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),
                new Vector3(1f, 0f, 1f),
                new Vector3(0f, 0f, 1f),
                // 4,5,6
                new Vector3(0f, 0f, 0f),
                new Vector3(0.5f, 1f, 0.5f),
                new Vector3(1f, 0f, 0f),
                // 7,8,9
                new Vector3(1f, 0f, 0f),
                new Vector3(0.5f, 1f, 0.5f),
                new Vector3(1f, 0f, 1f),
                // 10,11,12
                new Vector3(1f, 0f, 1f),
                new Vector3(0.5f, 1f, 0.5f),
                new Vector3(0f, 0f, 1f),
                // 13,14,15
                new Vector3(0f, 0f, 1f),
                new Vector3(0.5f, 1f, 0.5f),
                new Vector3(0f, 0f, 0f),
            };

            //頂点のインデックスを整える
            //この順番を参照して面ができあがる

            //ポリゴン
            //ポリゴンとは複数の頂点と頂点を線で繋げた図形のことを指します。
            //Unityでは基本的に三角形で1つのポリゴンを成します。
            //また、時計回り側を表面とします。
            var triangles = new List<int>
            {
                //底面
                3, 0, 1,
                3, 1, 2,
                //側面
                4, 5, 6,
                7, 8, 9,
                10, 11, 12,
                13, 14, 15,
            };

            //複数のポリゴンをまとまったものをメッシュ
            //メッシュには頂点情報の他に、
            //・頂点インデックス(何番目の頂点か)
            //・法線情報(ポリゴンの表面に対して垂直な直線)
            //・頂点カラーなどが含まれます。


            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();//メッシュフィルターとマテリアルを合わせて計算し、描画してくれる
            meshRenderer.material = mat;

            var mesh = new Mesh();          //メッシュを作成            
            mesh.Clear();                   //初期化            
            mesh.SetVertices(vertices);     //メッシュに頂点を登録                       
            mesh.SetTriangles(triangles, 0);//メッシュにインデックスリストを登録する  //第二引数はサブメッシュ(複数マテリアル割り当てる場合に使われるメッシュ)指定用

            //各頂点に色の情報を付与
            //各頂点に色情報が付与され、ラスタイズ(ピクセル間同士の色の補間)が行われます
            mesh.colors = new[]
            {
                Color.red,  //0
                Color.green,
                Color.blue,
                Color.gray,//3
                Color.red,  //4
                Color.green,//5
                Color.blue, //6
                Color.gray,
                Color.red,
                Color.green,
                Color.blue,//10
                Color.gray,//11
                Color.red, //12
                Color.green,
                Color.blue,
                Color.gray//15
            };

            mesh.RecalculateNormals();      //法線の再計算  右回り正(左手座標系の正方向)          
            var meshFilter = GetComponent<MeshFilter>();//MeshFilterのアドレス取得
            meshFilter.mesh = mesh;         // 作成したメッシュを適応


        }
    }
}