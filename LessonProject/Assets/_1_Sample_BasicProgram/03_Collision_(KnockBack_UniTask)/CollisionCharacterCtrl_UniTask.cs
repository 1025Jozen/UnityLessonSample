using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KnockBack
{
    public class CollisionCharacterCtrl_UniTask : MonoBehaviour
    {
        private Vector3 knockbackVelocity = Vector3.zero;
        CharacterController characterController;

        private void OnControllerColliderHit(ControllerColliderHit hit) {
            if (hit.gameObject.CompareTag("Enemy")) {
                Debug.Log("当たった");
                Damage();
            }
        }

        private void Start() {
            characterController = GetComponent<CharacterController>();
        }

        private void Update() {

            if (knockbackVelocity != Vector3.zero) {
                characterController.Move(knockbackVelocity * Time.deltaTime);
            }
        }

        public async void Damage() {
            knockbackVelocity = (-transform.right * 5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));//0.5秒待ってからベクトル初期化
            knockbackVelocity = Vector3.zero;
        }


        //レイキャストを用いた当たり判定
        //private void Update() {
        //    // CharacterControllerの当たり判定を取得
        //    Collider[] hitColliders = Physics.OverlapBox(
        //        characterController.bounds.center,  // 当たり判定の中心座標
        //        characterController.bounds.extents, // 当たり判定のサイズの半分
        //        Quaternion.identity       // 回転なし
        //    );
        //    // 当たり判定を処理する
        //    foreach (Collider hitCollider in hitColliders) {
        //        // ここで当たり判定を処理する
        //    }
        //}
    }
}