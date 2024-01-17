using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic1
{
    //State=クラスverの引数みたいなもの  「(State) は Enumを継承していること」という制約条件をつけた
    public abstract class BaseState<State> where State :Enum
    {
        public BaseState(State key) {
            StateKey = key;
        }
        public State StateKey { get; private set; } //状態


        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();

        public abstract State GetNextState();

        public abstract void OnTriggerEnter(Collider other);
        public abstract void OnTriggerStay(Collider other);
        public abstract void OnTriggerExit(Collider other);

        //public abstract void OnCollisionEnter();
        //public abstract void OnCollisionStay();
        //public abstract void OnCollisionExit();
    }
}

