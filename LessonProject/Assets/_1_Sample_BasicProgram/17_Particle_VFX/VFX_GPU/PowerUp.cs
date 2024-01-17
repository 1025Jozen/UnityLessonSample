using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace PowerUp {
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] VisualEffect levelUp;

        private bool levelingUp;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (anim != null) {
                if (Input.GetButtonDown("Fire1") && ! levelingUp) {
                    anim.SetTrigger("LevelUp");

                    if (levelUp != null) {
                        levelUp.Play();
                    }

                    levelingUp = true;
                    StartCoroutine(ReserBool  (1f));
                }
            }
        }

        IEnumerator ReserBool(float delay = 0.1f) {
            yield return new WaitForSeconds(delay);
            levelingUp = !levelingUp;
        }
    }
}

