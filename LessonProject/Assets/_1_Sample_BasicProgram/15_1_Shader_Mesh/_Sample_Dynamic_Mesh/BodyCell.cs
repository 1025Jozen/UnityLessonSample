using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dynamic_Mesh
{
    public class BodyCell : MonoBehaviour
    {
        public BodyCell prev, next;
        private Vector3 offset;
        public float speed;
        private Rigidbody rb;

        private void Start() {
            //現在地-offsetで進行方向を取得するため
            offset = transform.position;
            rb = GetComponent<Rigidbody>();
            if (prev != null)
                prev.next = this;
        }


        private void FixedUpdate() {
            //頭セル以外は前のセルに追従する
            if (prev != null)
                BodyCellMove();
        }


        void BodyCellMove() {
            //前セルとの距離を取得
            //距離が一定以上のときに前セルとの差分方向に移動
            Vector3 dis = prev.transform.position - transform.position;
            if (dis.magnitude > 2f) {
                rb.velocity = dis * speed * Time.deltaTime;
            }
            //単位時間での移動方向を取得
            Vector3 diff = transform.position - offset;

            //移動距離が一定以上でその方向を向く
            if (diff.magnitude > 0.05f) {
                if (next == null)
                    diff.y = 0;
                transform.rotation = Quaternion.LookRotation(diff);
            }
            offset = transform.position;
        }



        public List<Vector3> AddPositionToList(List<Vector3> meshPos) {
            meshPos.Add(transform.position);

            if (next != null)
                return next.AddPositionToList(meshPos);
            else
                return meshPos;
        }

    }
}