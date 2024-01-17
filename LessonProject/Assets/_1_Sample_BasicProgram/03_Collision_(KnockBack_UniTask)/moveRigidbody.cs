using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnockBack {
    public class moveRigidbody : MonoBehaviour
    {
        Rigidbody rb;
        Vector3 force;

        void Start() {
            rb    = this.GetComponent<Rigidbody>(); // rigidbodyを取得
            force = new Vector3(1.0f, 0.0f, 0.0f);  // 力を設定
        }

        void FixedUpdate() {                        
            rb.AddForce(force);  // 力を加える
        }
    }
}

