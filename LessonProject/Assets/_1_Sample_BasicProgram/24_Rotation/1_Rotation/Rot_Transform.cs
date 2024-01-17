using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rot_Sample {
    public class Rot_Transform : MonoBehaviour
    {
        float inputX;
        [SerializeField]float yRotSpeed = 100f;
        Vector3 yRot;

        void Update() {
            inputX = Input.GetAxis("Horizontal");

            //①
            //transform.localEulerAngles += new Vector3(0, inputX, 0) * yRotSpeed * Time.deltaTime;

            // 1秒で100度回転　して欲しいとする
            //Δt秒で  ?度回転  すれば良いか?

            //②Time.deltaTime
            yRot += new Vector3(0, inputX * Time.deltaTime * yRotSpeed, 0);
            //transform.rotation = Quaternion.Euler(yRot);//yRotの方向を見続ける //入力がなければ同じ方向を見る //ワールド座標
            transform.localRotation = Quaternion.Euler(yRot);//ローカル座標

            //③Time.deltaTime は毎回値が違う　　回転の速度の変化量に直接Time.deltaTimeが乗算される　　　(こっちは挙動がバグる)
            //例えば　 yRot = 1 だとする　この時　Time.deltaTime　の値が0.01だとすると　(0度から数えて)回転角度は　0.01
            //次に　　yRot = 2だとする　　この時　Time.deltaTime  の値が0.0045だとすると (0度から数えて)回転角度は 0.005 と角度が進まない こういったことが起こるのでバグる
            //yRot += new Vector3(0, inputX, 0);
            //transform.rotation = Quaternion.Euler(yRot * Time.deltaTime * yRotSpeed);//入力がなくても回転し続けてしまうので注意

            //④Unityが用意してくれてるこっちのメソッド使えばうまくいく 
            //transform.Rotate(new Vector3(0, inputX, 0) * yRotSpeed * Time.deltaTime);//ローカル座標　Rotate (1.0f, 1.0f, 1.0f, Space.World )でワールド
        }

        private void FixedUpdate() {

            //こっちはゲーム内時間が一定
            //rigidbodyなどの物理演算処理をするときはこっちの方がいい場合も
            //③
            //yRot += new Vector3(0, inputX, 0);
            //transform.rotation = Quaternion.Euler(yRot * Time.fixedDeltaTime * yRotSpeed);
        }
    }
}


