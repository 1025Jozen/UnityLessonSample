using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSample {
    public class Water : MonoBehaviour
    {
       
        private CharacterController cc;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject RippleCamera;//波紋だけをうつすカメラ
        [SerializeField] ParticleSystem ripple;


        private float VelocityXZ, VelocityY;//位置の差から速さを割り出す
        private Vector3 PlayerPos;//前のポジション保持

        private RaycastHit isGround;
        private bool inWater;

        void Start() {
            cc = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update() {
            PlayerMovement();
            ripple.transform.position = transform.position;

            if (!isGround.collider) {
                cc.Move(new Vector3(0, -10 * Time.deltaTime, 0));
            }

            RippleCamera.transform.position = transform.position + Vector3.up * 10;

            Shader.SetGlobalVector("_Player", transform.position);//Shader
        }

        void PlayerMovement() {
            Vector3 camRight   = mainCamera.transform.right;
            Vector3 camForward = mainCamera.transform.forward;
            camRight.y = 0;
            camForward.y = 0;


            VelocityXZ = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(PlayerPos.x, 0, PlayerPos.z));
            VelocityY  = Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, PlayerPos.y, 0));
            PlayerPos = transform.position;


            Vector3 move = camRight.normalized * Input.GetAxis("Horizontal") + camForward.normalized * Input.GetAxis("Vertical");
            cc.Move(move.normalized * Time.deltaTime * 3 * ((Input.GetKey(KeyCode.LeftShift) ? 2 : 1)));
            if (move.magnitude > 0) transform.forward = move.normalized;//向きを変える

            //地面との当たり判定　isGroundに情報が入る
            Physics.Raycast(transform.position, Vector3.down, out isGround, 2.7f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, Vector3.down * 2.7f);

            //水との接触判定
            float height = cc.height + cc.radius;
            inWater = Physics.Raycast(transform.position + Vector3.up * height, Vector3.down, height * 2, LayerMask.GetMask("Water"));
            Debug.DrawRay(transform.position + Vector3.up * height, Vector3.down * height);
        }

        //forの回数で生成する箇所を調整する

                          //スタート位置　終わり　生成角度間隔　　　　スピード　  サイズ　      寿命
        void CreateRipple(int Start, int End, int Delta, float Speed, float Size, float Lifetime) {
            Vector3 forward = ripple.transform.eulerAngles;//エフェクトの向きを保存
            forward.y = Start;//y軸回転させるためのスタート位置を設定
            ripple.transform.eulerAngles = forward;//y軸回転させる時のパーティクルの位置を設定

            //for (int i = 0; i < 360; i += 9) {
            for (int i = Start; i < End; i += Delta) {
                //パラメーター
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

                emitParams.position = transform.position + ripple.transform.forward;//位置           
                emitParams.velocity = ripple.transform.forward * Speed; //速さ     
                emitParams.startSize = Size;        //サイズ            
                emitParams.startLifetime = Lifetime;//寿命            
                emitParams.startColor = Color.white;//色
                                                    //emitParams.applyShapeToPosition = true;
                                                    //位置　速さ　サイズ　寿命　色
                                                    //ripple.Emit(transform.position + ripple.transform.forward, ripple.transform.forward, 2, 3, Color.white);
                ripple.Emit(emitParams, 1);
                ripple.transform.eulerAngles += Vector3.up * 3;
            }
        }



        private void OnTriggerEnter(Collider other) {

            ////水に入った時
            //if (other.gameObject.layer == 4 ) {
            //    Debug.Log("水中に入った");
            //    CreateRipple(-180, 180, 3, 2, 1f, 2);
            //}

        }


        private void OnTriggerStay(Collider other) {
            //水に入っている間 経過した描画フレーム総数などに合わせてParticle生成
            if (other.gameObject.layer == 4 && VelocityXZ > 0.001f && Time.renderedFrameCount % 3 == 0) {
                int y = (int)transform.eulerAngles.y;
                //出す位置をyの回転角度に合わせる
                //スタート位置　終わり 間隔 スピード　サイズ　寿命        前方0度
                CreateRipple(y - 270, y - 90, 3, 5, 0.5f, 1);
            }
        }

        private void OnTriggerExit(Collider other) {
            ////水から出た時
            //if (other.gameObject.layer == 4) {
            //    CreateRipple(-180, 180, 2, 5, 3f, 3);
            //}
        }
    }
}

