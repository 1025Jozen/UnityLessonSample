using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Anim_Sample { 
    public class Anima_Ctrl : MonoBehaviour
    {
        Animator anim;


        void Start()
        {
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            float progress = stateInfo.normalizedTime;

            Debug.Log(progress);
        }
    }
}