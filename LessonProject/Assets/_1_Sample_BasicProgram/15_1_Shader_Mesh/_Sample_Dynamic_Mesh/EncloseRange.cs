using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dynamic_Mesh
{
    public class EncloseRange : MonoBehaviour
    {
        private void OnTriggerStay(Collider other) {
            if (other.GetComponent<BodyCell>() == null)
                if (other.name == "Enemy") {
                    //otherを消す処理⑥
                    Debug.Log("当たった");
                    Destroy(other.gameObject);
                }
        }

        private void OnCollisionStay(Collision collision)
        {
            //Debug.Log("当たった");

            if (collision.gameObject.GetComponent<BodyCell>() == null) {
                Debug.Log("何かに当たった");
                if (collision.gameObject.name == "Enemy") {
                    //otherを消す処理⑥
                    Debug.Log("消したいものに当たった");
                    Destroy(collision.gameObject);
                }
            }
            
        }
    }
}