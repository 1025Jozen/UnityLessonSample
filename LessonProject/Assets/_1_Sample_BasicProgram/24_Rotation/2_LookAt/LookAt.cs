using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LookAt_Sample {
    public class LookAt : MonoBehaviour
    {
        [SerializeField] GameObject target;
        public float rotationSpeed = 1f;                //回転速度

        void Update() {
            Look();
        }

        private void Look() {
            //対象物までのベクトル
            Vector3 dir = (target.transform.position - this.transform.position).normalized;

            //y座標は考慮しない　(y座標を考慮した場合、体ごと対象物の方を向くので時と場合によってyは無視しない)
            Vector3 LookVec = new Vector3(dir.x, 0f, dir.z);

            //回転行列
            Quaternion lookAtRotation = Quaternion.LookRotation(LookVec, Vector3.up);

            //第１引数を第２引数まで第３引数のスピードで動かす
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

