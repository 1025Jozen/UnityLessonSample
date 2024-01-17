using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ragdoll_Sample {
    public class Ragdoll : MonoBehaviour
    {
        public List<Collider> ragdollParts = new List<Collider>();
        Rigidbody rb;
        CharacterController chara;
        Animator anim;

        bool attackTrigger;

        public AnimationCurve movementCurve; // キャラクターの移動曲線
        public float movementSpeed = 5.0f;   // 移動速度
        private float elapsedTime = 0f;      // 経過時間

        public float attackForce = 1.0f;


        IEnumerator Start() {
            rb    = GetComponent<Rigidbody>();
            anim  = GetComponent<Animator>();
            chara = GetComponent<CharacterController>();
            SetRagdoll();//初めはアニメーションなどがつけられるように、コライダーのisTriggerをオンにして干渉しないようにする 

            yield return new WaitForSeconds(1f);
            //ノックバックして
            anim.SetTrigger("Hit");
            attackTrigger = true;
            yield return new WaitForSeconds(1f);
            //しばらくしたら倒れる
            TurnOnRagdoll();
        }

        

        void Update() {
            //アニメーションの進行状況でragdoll化させてやってもいいがragdollの紹介が目的なので割愛

            // アニメーションの現在の状態を取得
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);//0はレイヤー
            // アニメーションの進行度を取得
            float progress = stateInfo.normalizedTime;//２周目以降があるならば1.0よりも大きな値になる。
            // 進行度を表示
            Debug.Log("Animation Progress: " + progress);
            Debug.Log("Animation 経過時間: "  + progress * stateInfo.length);







            if (attackTrigger) {
                float forceMultiplier = movementCurve.Evaluate(elapsedTime);//AnimationCurveに基づいて経過時間によって移動量を取得
                rb.AddForce(Vector3.back * attackForce * forceMultiplier);
                elapsedTime += Time.deltaTime;                            // 経過時間を増やす
                if (elapsedTime >= movementCurve.keys[movementCurve.length - 1].time) {//movementCurve全てのキーフレームを取得し　最後のキーフレームを取得　その時間を取得
                    attackTrigger = false;

                    if (rb.velocity.magnitude < 0.5f) {//rigidbodyの影響で動き続けてしまうのである程度で止める
                        rb.velocity = Vector3.zero;
                    }
                }
            }
        }


        void SetRagdoll() {
            Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders) {
                if (col.gameObject != this.gameObject) {
                    col.isTrigger = true;
                    col.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    ragdollParts.Add(col); ;
                }
            }
        }

        void TurnOnRagdoll() {        
            rb.isKinematic = true;
            //rb.velocity   = Vector3.zero;
            //rb.useGravity = false;
            anim.enabled  = false;
            anim.avatar   = null;
            foreach (Collider col in ragdollParts) {
                col.isTrigger = false;
                col.attachedRigidbody.velocity = Vector3.zero;
                col.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}

