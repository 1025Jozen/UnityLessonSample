using UnityEngine;
using DG.Tweening;


namespace KnockBack
{
    public class CollisionCharacterCtrl_DOTween : MonoBehaviour
    {
        private Vector3 knockbackVelocity = Vector3.zero;
        CharacterController characterController;

        private void Start() {
            characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            if (knockbackVelocity != Vector3.zero) {
                characterController.Move(knockbackVelocity * Time.deltaTime);
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit) {
            if (hit.gameObject.CompareTag("Enemy")) {
                Debug.Log("当たった");
                Damage();
            }
        }

        //NavMeshAgentでも一緒

        public void Damage() {
            //アニメーション
            DOTween.To(
                () => transform.position,//第1引数で「どこから」

                //第2引数のラムダ式で「何に」
                //引数のvには「第1引数と第3引数の間の座標」が入ります。
                //Move()で指定する値は速度(velocity)
                //なので「v - 現在位置」で次の地点から現在位置までの移動量を取得し、それをMove()の移動に利用します。
                v => {
                    Vector3 velocity = (v - transform.position) * Time.deltaTime;
                    characterController.Move(velocity);//
                },

                transform.position + (-transform.right * 5f), //第3引数で「どこまで」
                0.5f            //ゲームに応じて第3引数で距離を、第4引数でのけぞり時間を調整
            );

        }
    }
}

//ControllerColliderHit　変数例
//collider      コントローラーにヒットしたコライダー
//controller	ヒットしたコライダーのコントローラー
//gameObject	コントローラーにヒットしたゲームオブジェクト
//moveDirection	衝突が起こったとき CharacterController が動く方向
//moveLength	コライダーにヒットするまでに、キャラクターがどのくらいの距離を移動したか。
//normal	    ワールド空間で衝突した表面の法線
//point	        ワールド空間における衝突点
//rigidbody	    コントローラーがヒットしたリジッドボディ
//transform	    コントローラーがヒットしたトランスフォームこれらの情報はCollisionオブジェクトを介して取得でき、
//              衝突時の詳細な情報を扱うのに役立ちます。