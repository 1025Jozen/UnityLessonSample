using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic
{
    //ここが大元
    public class PlayerStateMachine:MonoBehaviour
    {
        public PlayerStateBase currentState; //現在のステート
        Dictionary<string, PlayerStateBase> states;       //全てのステート

        private void Awake()
        {
            states.Add("Idle",new PlayerIdle());
            states.Add("Walk",new PlayerWalk());

            currentState = states["Idle"];
            currentState.EnterState();
        }

        private void Update()
        {
            currentState.UpdateState();
        }

        void SwitchState(PlayerStateBase newState) {
            currentState.EnterState();
            newState.EnterState();
            currentState = newState;
        }
    }
}