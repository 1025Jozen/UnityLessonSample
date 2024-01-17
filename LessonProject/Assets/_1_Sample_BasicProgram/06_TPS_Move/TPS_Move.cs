using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace TPS
{
    public class TPS_Move : MonoBehaviour
    {
        CharacterController characterController;
        InputManager_TPS inputManager;
        //[SerializeField]CinemachineFreeLook freeLookCam;

        //入力量
        float inputX;
        float inputZ;
        float inputMagnitude;

        Vector3 playerRotVector;     //最終的にキャラを回転させる方向
        Vector3 playerVelocity;      //最終的なプレイヤーの速度ベクトル


        [SerializeField] float charaRotSpeed  = 0.3f;//キャラの回転速度
        [SerializeField] float charaMoveSpeed = 9;   //キャラの速さ
        float allowCharaMoveValue  = 0.1f;           //一定以上の入力がある時のみ動かす
        bool inputMoveAllow;

        float gravity         = -9.81f;//重力

        /**/
        private void Awake()
        {
            
        }

        private void Start()
        {
            inputManager        = InputManager_TPS.Instance;    //入力情報取得できるインスタンス
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleGravity();
            GetInput();
            CalculatePlayerMovement();      //playerVelocity・playerRotVectorを計算
            PlayerMove(playerVelocity);     //引数の割合でキャラを動かす
            PlayerRotation(playerRotVector);//引数の方向にゆっくり回転させる
        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
            
        }

        /*地面についてるか*/
        public bool GroundCheck()
        {
            return characterController.isGrounded;
        }
        public void HandleGravity() {
            bool isFalling = playerVelocity.y <= 0.0f;//落ちている判定
            float fallCoefficient = 2.0f;             //上昇時にはこの係数は掛けない

            bool grounded = GroundCheck();

            float fallingMaxSpeed = -20;
            if (grounded) {//地面についてる時               
                playerVelocity.y = gravity;
            } else if (isFalling) {
                playerVelocity.y += Mathf.Max( gravity * fallCoefficient * Time.deltaTime , fallingMaxSpeed);
            } else {
                playerVelocity.y += gravity * Time.deltaTime;
            }
        }

        /*入力値を取得*/
        public void GetInput() {
            inputX = inputManager.inputMoveVector.x;
            inputZ = inputManager.inputMoveVector.z;
            inputMagnitude = new Vector2(inputX, inputZ).sqrMagnitude;
        }

        /*入力値を取得後、動かしたい方向をカメラの向きから把握し、動かすベクトルを算出*/
        public void CalculatePlayerMovement() {
            inputMoveAllow = inputMagnitude > allowCharaMoveValue;//閾値以上なら動かす判定
            if (!inputMoveAllow) {//入力がないときはy軸だけ動くように
                playerVelocity = new Vector3(0, playerVelocity.y, 0);
            } else {//カメラ基準で方向を決めて動かす
                Camera cam = Camera.main;
                Vector3 camForward = cam.transform.forward;//カメラから見て前　
                Vector3 camRight = cam.transform.right;  //カメラから見て右

                camForward.y = 0f;//xz座標だけみる
                camRight.y = 0f;//xz座標だけみる

                camForward.Normalize(); //正規化
                camRight.Normalize();
               
                playerRotVector = camForward * inputZ + camRight * inputX;//動かしたい方向
                Vector3 inputCharaVelocity = playerRotVector * charaMoveSpeed * ((inputManager.Run()) ? 3 : 1);

                playerVelocity = new Vector3(inputCharaVelocity.x, playerVelocity.y, inputCharaVelocity.z);
            }
        }



        /*　実際に動かす　*/
        public void PlayerMove(Vector3 playerVelocity) {//動かす
            characterController.Move(playerVelocity * Time.deltaTime);
        }
        public void PlayerRotation(Vector3 playerXZ_Direction) {//回転させる
            if (inputMoveAllow) {//動かす時に動かす方向に回転させる
                transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(playerXZ_Direction), charaRotSpeed
                    );//動かしたい向きにゆっくり回転させる
            }
        }




        //x軸周りの回転　freelookCameraで言うところのy軸
        //private void LimitFreeLookYAxis() {
        //    Debug.Log(freeLookCam.m_YAxis.Value);//0~1で管理されてる　0~180度で指示する感じ

        //    float currentYAngle = freeLookCam.m_YAxis.Value * 180f;
        //    float minYAngle = 0f;
        //    float maxYAngle = 180f;

        //    if (currentYAngle < minYAngle) {
        //        freeLookCam.m_YAxis.Value = minYAngle / 180f;
        //    } else if (currentYAngle > maxYAngle) {
        //        freeLookCam.m_YAxis.Value = maxYAngle / 180f;
        //    }
        //}
    }
}