using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rot_Sample {
    public class Rot_Rigidbody : MonoBehaviour
    {
        float inputX;

        [SerializeField] float torqueForce = 10f; // トルクの強さ

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update() {
            inputX = Input.GetAxis("Horizontal");

            
            //斜めにして回転させるとなると予測と違いが出てくると思う、
            //それを解消するにはトルクの物理的意味　角速度についての理解が必要になる
            //参考　→　Unity公式動画　　【Unity道場 2018】物理シミュレーション完全マスター
            //https://www.youtube.com/watch?v=Ju4ILgpuVHE&ab_channel=UnityJapan
            if (inputX > 0.1f || inputX < -0.1f) {
                var omega = new Vector3(0f,1f,0f);//角速度
                var Id = rb.inertiaTensor;
                var Ir = rb.inertiaTensorRotation;
                var IrI = Quaternion.Inverse(Ir);
                //T=I*omega
                //T        = Ir *      Id*IrI*omega
                var torque = Ir * Vector3.Scale(Id,IrI*omega);
                rb.AddTorque(inputX * torqueForce * torque , ForceMode.Impulse);
            }
           
        }



        //private void FixedUpdate() {
        //    if (inputX > 0.1f || inputX < -0.1f) {
        //        Vector3 torque = new Vector3(0, inputX * torqueForce, 0);// トルク(回転させる力)をY軸（上向き）に適用する
        //        //rb.AddTorque(torque,ForceMode.Force);
        //        rb.AddRelativeTorque(torque, ForceMode.Force);//ローカル座標

        //        //第２引数でモードの設定ができる
        //        //Force          設定された質量を考慮して、   継続的に力を与え続けるモード
        //        //Acceleration   設定された質量を考慮しないで、継続的に力を与えるモード
        //        //Impulse        設定された質量を考慮して、    瞬間的に力を与えるモード
        //        //VelocityChange 設定された質量を考慮しないで、瞬間的に力を与え続けるモード

        //        //Rigidbodyのメソッドについては　公式リファレンス参照
        //        //AddForceやAddExplosionForceなど他にもある
        //    }
        //}

    }
}


