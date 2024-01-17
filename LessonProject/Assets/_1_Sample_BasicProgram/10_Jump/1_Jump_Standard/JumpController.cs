using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace Jump {

    public class JumpController : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 playerVelocity;//プレイヤーを動かす際に使用するベクトル変数
        private bool isGrounded;
        private float initialJumpVelocity;
        private float gravity = -9.81f;

        public float jumpHeight = 2.0f;
        public float timeToJumpApex = 0.5f; // ジャンプの到達時間



        private void Start() {
            controller = GetComponent<CharacterController>();           
            initialJumpVelocity = CalculateJumpVelocity(jumpHeight, timeToJumpApex);// ジャンプ時に必要な初速を計算
        }
        private void Update() {
            //playerVelocity　の Vector3　の操作でキャラを動かしてる
            GroundCheck();   //地面に着地しているか
            HandleGravity(); //重力に関するメソッド
            HandleJump();    //ジャンプに関するメソッド            
            PlayerMove();    //最終的にキャラの移動を行う
            Debug.Log(playerVelocity);
        }



        // ジャンプの初速を計算する関数
        private float CalculateJumpVelocity(float jumpHeight, float timeToJumpApex) {
            // ジャンプの初速を計算する式(普通の物理法則から逆算したやつ)　// 初速 = (2 * 高さ) / 到達時間 - 重力 * 到達時間 / 2
            float gravityValue = Mathf.Abs(gravity);
            float initialVelocity = (2 * jumpHeight) / timeToJumpApex - (gravityValue * timeToJumpApex) / 2;
            return initialVelocity;
        }

        void GroundCheck() {
            isGrounded = controller.isGrounded;
            Debug.Log(isGrounded);
        }
        void HandleJump() {
            if (isGrounded) {
                if (Input.GetButtonDown("Jump")) {// 地面にいる場合、ジャンプ処理を行う                   
                    playerVelocity.y = initialJumpVelocity;// 初速を使ってジャンプ(ベクトルのy成分に代入)
                }
            }

        }
        void HandleGravity() {
            if (!isGrounded) {
                float g = gravity;
                playerVelocity.y += g * Time.deltaTime;// 下方向のベクトルへ作用　
            }
            if (isGrounded) {
                playerVelocity.y = gravity;// 下方向のベクトルは一定
            }
        }

        
        public void PlayerMove() {
            controller.Move(playerVelocity * Time.deltaTime);//キャラを動かす
        }
    }

}

