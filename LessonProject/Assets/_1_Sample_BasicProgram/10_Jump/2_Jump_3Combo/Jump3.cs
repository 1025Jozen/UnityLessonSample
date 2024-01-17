using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jump3 {
    public class Jump3 : MonoBehaviour
    {

        CharacterController characterController;
        Animator anim;

        Vector3 moveDirection;


        bool isJumping;//ジャンプ中
        [SerializeField] float gravity            = -10f;   //重力(に相当するもの)
        [SerializeField] float jumpHeight         = 12f;    //1回目のジャンプの高さの目安 今回の設定上gravityよりは大きくしないとダメかも
        [SerializeField] float offsetSecoundHight = 2f;     //2回目のジャンプの高さの目安+
        [SerializeField] float offsetThirdHight   = 4f;     //3回目のジャンプの高さの目安+
        int jumpCount = 0;
        [SerializeField] float fallingMaxSpeed = -20;
        [SerializeField] float timeToJumpApex = 1f;  //ジャンプして頂点までたどり着く時間
        [SerializeField] float fallMultiplier = 1.5f;//落下係数（降りる時だけ速くすることでフワッと感をなくす）


        Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();//ジャンプ初速
        Dictionary<int, float> jumpGravities         = new Dictionary<int, float>();//ジャンプ時にかける重力

        Coroutine jumpResetRoutine = null;



        int isIdling_Hash;
        int isJumping_Hash;
        int jumpCount_Hash;


        

        



        /*                    */

        void Start() {
            //アドレス取得
            characterController = GetComponent<CharacterController>();
            anim                = GetComponent<Animator>();

            //Hash値設定
            isIdling_Hash  = Animator.StringToHash("isIdling");
            isJumping_Hash = Animator.StringToHash("isJumping");
            jumpCount_Hash = Animator.StringToHash("jumpCount");

            //パラメーター初期設定
            CalculateJumpVelocity();
        }


        void Update() {
            ////Debug.Log(isJumping);
            //if (Input.GetButtonDown("Jump")) {
            //    moveDirection.y = jumpForce;
            //}

            handleGravity();   //重力
            handleJump();      //ジャンプ
            //handleAnimation(); //アニメーション

            //最終的に動かす
            characterController.Move(moveDirection * Time.deltaTime);
        }

        /*                    */

        //ジャンプの初速をそれぞれ設定
        void CalculateJumpVelocity() {

            //ジャンプの高さ　頂点までにかける時間　によってジャンプ力を決めてみる


            float gravityValue = gravity;
            // ジャンプの初速を計算する式(普通の物理法則から逆算したやつ)　// 初速 = (2 * 高さ) / 到達時間 - 重力 * 到達時間 / 2
            //重力大きすぎるとジャンプできないかも
            float initialVelocity = (jumpHeight) / timeToJumpApex + (gravityValue * timeToJumpApex) / 2;
            //  10 / 1  -  10 /2
            float secondGravityValue = gravityValue * 1.25f;        
            float secondJumpVelocity = (jumpHeight + offsetSecoundHight) / timeToJumpApex + (secondGravityValue * timeToJumpApex) / 2;

            float thirdGravityValue = gravityValue * 1.5f;            
            float thirdJumpVelocity = (jumpHeight + offsetThirdHight) / timeToJumpApex + (thirdGravityValue * timeToJumpApex) / 2;

            //ジャンプの重力に関する辞書 
            jumpGravities.Add(0, gravityValue);
            jumpGravities.Add(1, secondGravityValue);
            jumpGravities.Add(2, thirdGravityValue);

            //ジャンプの初速に関する辞書
            initialJumpVelocities.Add(0, initialVelocity);
            initialJumpVelocities.Add(1, secondJumpVelocity);
            initialJumpVelocities.Add(2, thirdJumpVelocity);
            //Debug.Log(initialJumpVelocities[0]);
        }



        /*                    */
             
        void handleGravity() {　　　　//下向き方向の変位について(重力相当)
            bool isFalling = moveDirection.y <= 0.0f || !Input.GetButton("Jump");
            //Debug.Log($"地面に着いているかどうか {characterController.isGrounded}　");
            if (characterController.isGrounded) {//地面に着いてる上で
                moveDirection.y = gravity;//地面にいる時は一定の下方向に
                if (isJumping) {
                    anim.SetBool(isIdling_Hash, true);  //StatePatternで書き直すなら　IdleステートにEnterした時に書いてもいい
                    anim.SetBool(isJumping_Hash, false);//StatePatternで書き直すなら　JumpステートからExitする時に書いてもいい
                    //Debug.Log(jumpCount);
                    jumpResetRoutine = StartCoroutine(JumpResetRoutine()); //ジャンプ回数を0回にリセット　どういう時に？　もしある程度の時間以内　に　もう一回ボタンを押したらリセットしない　(ジャンプの回数が3回目ならリセット)
                }
                

            } else if (isFalling) {//落下判定時　　落下速度を上げてみる

                moveDirection.y += Mathf.Max(jumpGravities[jumpCount % 3] * fallMultiplier * Time.deltaTime , fallingMaxSpeed);

            } else {//上昇中など
                moveDirection.y += gravity * Time.deltaTime;//それ以外はgravity分を足していく　gt 速度　
            }
        }

        IEnumerator JumpResetRoutine() {//ジャンプ回数をリセット
            float jumpStopCountTimingOffset = 0.5f;

            yield return new WaitForSeconds(jumpStopCountTimingOffset);

            jumpCount = 0;
        }






        void handleJump() {//ジャンプさせる

            if (characterController.isGrounded && !isJumping && Input.GetButton("Jump")) {
                isJumping = true;

                if ( jumpResetRoutine != null ) {//　ジャンプ回数リセットコルーチン変数の中身が　nullじゃなかったら
                    StopCoroutine(jumpResetRoutine);//コルーチンを止める　非同期で走っていた処理を止める
                    Debug.Log("ジャンプボタンが押されたのでコルーチンを止める");
                }
                //ジャンプカウントによって　初速を決める
                moveDirection.y = initialJumpVelocities[jumpCount % 3];
                jumpCount = jumpCount % 3;//ジャンプカウントが0 なら　1回目のジャンプ処理をこれからするという意味
                                          //ジャンプカウントが1 なら　2回目のジャンプ処理をこれからするという意味
                                          //ジャンプカウントが2 なら　3回目のジャンプ処理をこれからするという意味
                anim.SetBool(isIdling_Hash, false);//StatePatternで書き直すなら　IdleステートのExitに相当するところに書いてもいい   
                handleAnimation();                 //StatePatternで書き直すなら　JumpステートのEnterに相当するところに書いてもいい
                jumpCount += 1;
                Debug.Log(jumpCount);
            } else if (!Input.GetButton("Jump") && isJumping && characterController.isGrounded) {
                isJumping = false;
            }
        }






        void handleAnimation() {//ジャンプの回数によってアニメーションを変更
            anim.SetBool(isJumping_Hash, true);
            if (jumpCount == 0) {
                anim.SetInteger(jumpCount_Hash, jumpCount);
            } else if (jumpCount == 1) {
                anim.SetInteger(jumpCount_Hash, jumpCount);
            } else if (jumpCount == 2) {
                anim.SetInteger(jumpCount_Hash, jumpCount);
            }
        }
    }
}

