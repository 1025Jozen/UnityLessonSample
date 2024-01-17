
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic1
{
    public abstract class StateManager<State> :MonoBehaviour where State :Enum
    {
        protected Dictionary<State, BaseState<State>> states = new Dictionary<State, BaseState<State>>();

        protected　BaseState<State> currentState;

        protected bool IsTransitioningState = false;//遷移中かどうか
        //ゲームのフレームレートや処理時間によってはUpdate内の関数が複数回呼ばれ、複数回遷移してしまう可能性がある

        void Awake() { }

        void Start(){
            currentState.EnterState();
        }

        void Update(){
            State nextStateKey = currentState.GetNextState();

            if (nextStateKey.Equals(currentState.StateKey)) {
                currentState.UpdateState();
            } else {
                TransitionToState(nextStateKey);//ActiveなStateをExitして　次のStateにEnter 次のStateがActiveになる
            }
            
        }

        public void TransitionToState(State stateKey) {
            IsTransitioningState = true;//遷移開始
            currentState.ExitState();
            currentState = states[stateKey];
            currentState.EnterState();
            IsTransitioningState = false;//遷移終了
        }

        private void OnTriggerEnter(Collider other){
            currentState.OnTriggerEnter(other);
        }
        private void OnTriggerStay(Collider other) {
            currentState.OnTriggerStay(other);
        }
        private void OnTriggerExit(Collider other) {
            currentState.OnTriggerExit(other);
        }
    }
}