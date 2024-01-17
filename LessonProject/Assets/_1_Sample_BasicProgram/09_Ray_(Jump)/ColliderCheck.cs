using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ray_Sample
{
    public class ColliderCheck : MonoBehaviour
    {
        CharacterController cc;
        private RaycastHit isRayHit;
        Collider[] hitColliders;
        float height;
        Vector3 startPosition;


        void Start() {
            cc = GetComponent<CharacterController>();
            height = cc.height / 2;// + cc.radius;
            startPosition = transform.position + new Vector3(0,height,0);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Vector3 startPosition = transform.position + new Vector3(0,0.9f,0);
            Gizmos.DrawSphere(startPosition, 0.1f);

            Gizmos.color = Color.blue;
            Vector3 endPosition = startPosition + Vector3.down * (0.9f + 0.01f);
            Gizmos.DrawSphere(endPosition, .1f);

            //Gizmos.DrawLine(startPosition, endPosition);
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawRay(startPosition , Vector3.down * (0.9f + 0.01f));//位置と向き

            Gizmos.DrawLine(startPosition, endPosition);
        }

        /*   */ 

        void ConfirmRayInfo() {
            ////地面との当たり判定　に情報が入る
            Physics.Raycast(
                transform.position + new Vector3(0, 0.9f, 0),//始点
                Vector3.down,//下方向
                out isRayHit,//レイ情報　保存
                0.9f + 0.1f  //どれくらい
            );
            Debug.DrawRay(transform.position + new Vector3(0, 0.9f, 0), Vector3.down * (0.9f + 0.01f), Color.green);
            
        }

        void ConfirmSphreHitInfo() {//別の確認方法
            float radius = 0.01f;
            hitColliders = Physics.OverlapSphere(transform.position, radius);
        }

        /*   */

        public CollisionResults GetPlayerCollisionResults() {
            CollisionResults results = new CollisionResults();//構造体を実体化

            //ConfirmSphreHitInfo();　　
            //foreach (var hitCollider in hitColliders) {
            //    if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            //        results.IsGrounded = true;
            //    }
            //    //else if (isRayHit.collider.gameObject.layer == LayerMask.GetMask("Water")) {
            //    //    results.InWater = true;
            //    //}
            //    else {
            //        results.IsGrounded = false;
            //        //results.InWater = false;
            //    }
            //}

            ConfirmRayInfo();//レイを飛ばし、　レイ情報確認　isRayHitにレイ情報を入れた



            if (isRayHit.collider != null) {
                //Debug.Log("何かに当たった");
                if (isRayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                    //Debug.Log("地面に当たった");
                    results.IsGrounded = true;
                } else {
                    //Debug.Log("地面以外に当たった");
                    results.IsGrounded = false;
                }

                if (isRayHit.collider.gameObject.layer == LayerMask.GetMask("Water")) {
                    results.InWater = true;
                }
                else {
                    results.InWater = false;
                }
            } else {
                //Debug.Log("何にも当たってない");
                results.IsGrounded = false;
                results.InWater = false;
            }
            return results;
        }       
    }
}
