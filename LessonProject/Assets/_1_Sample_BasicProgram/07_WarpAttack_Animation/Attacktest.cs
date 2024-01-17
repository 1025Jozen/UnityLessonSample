using UnityEngine;

namespace WarpAttack
{

    public class Attacktest : MonoBehaviour
    {
        Animator anim;

        void Start() {
            anim = GetComponent<Animator>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                anim.SetTrigger("WarpAttack");
            }
        }
        public void Attack() {
            GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
            Destroy(clone.GetComponent<Animator>());
        }
    }

}