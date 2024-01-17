using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineBehaviourSample
{
    public class AnimatorExample : MonoBehaviour
    {
        Animator animator;

        public int k = 35; 

        void Start() {
            animator = GetComponent<Animator>();
            var stateMachineSample = animator.GetBehaviour<StateMachineBehaviourSample1>();
            Debug.Log(stateMachineSample.i);
        }
    }
}