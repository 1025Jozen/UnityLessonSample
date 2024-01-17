using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dynamic_Mesh {
    public class Player : MonoBehaviour
    {
        //移動関係の変数
        [SerializeField]
        private float speed, inputX, inputZ;
        private Vector3 moveDirection;
        private Rigidbody rb;

        //Mesh生成関係の変数
        private BodyCell headCell;

        [SerializeField]
        private MeshCollider meshCollider;
        [SerializeField]
        private Material mat;

        void Start() {
            //RigidBody、BodyCellの取得
            rb       = GetComponent<Rigidbody>();
            headCell = GetComponent<BodyCell>();
        }

        void Update() {
            //方向ベクトルを入力に対応させる
            inputX = Input.GetAxisRaw("Horizontal");
            inputZ = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(inputX, 0, inputZ);
        }

        private void FixedUpdate() {
            //ベクトルの方向に移動する
            rb.velocity = moveDirection * speed * Time.deltaTime;
        }



        private void OnCollisionEnter(Collision collision) {
            var Cell = collision.collider.GetComponent<BodyCell>();
            //nextがnullのセルは尻尾セルだけ
            if (Cell != null)
                if (Cell.next == null) {
                    MakeEncloseMesh();
                }
        }


        //メッシュ生成関数　生成したメッシュを返す
        //Vector3型のListに生成するメッシュの頂点を追加する
        void MakeEncloseMesh() {
            //①②の処理
            var meshPos = new List<Vector3>();
            headCell.AddPositionToList(meshPos);

            //③のCreateMeshの呼び出し・代入
            var mesh = CreateMesh(meshPos);
            //meshColliderにはEditor上でEncloseRangeのアタッチされたオブジェクトをアタッチ
            meshCollider.sharedMesh = mesh;
            meshCollider.transform.GetComponent<MeshFilter>().mesh = mesh;
            meshCollider.transform.GetComponent<MeshRenderer>().sharedMaterial = mat;
        }

        //Mesh生成
        Mesh CreateMesh(List<Vector3> pos) {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[pos.Count];
            int[] triangles = new int[(pos.Count - 2) * 3];

            for (int i = 0; i < pos.Count; i++)
                vertices[i] = pos[i];
            int j = 0;
            for (int i = 0; i < (pos.Count - 2) * 3; i++) {
                if (i > 0 && i % 3 == 0) {
                    triangles[i] = 0;
                    j += 2;
                } else
                    triangles[i] = i - j;
                //Debug.Log(triangles[i]);
            }

            //以下にメッシュ生成処理
            //vertices,trianglesの代入と法線再計算（RecalculateNormals）
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}

