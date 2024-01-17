using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic
{
    //ここが大元
    public class EnemyStateMachine:MonoBehaviour
    {
        public EnemyStateBase currentState; //現在のステート
        Dictionary<string, EnemyStateBase> states;       //全てのステート

        private void Awake()
        {
            states.Add("Patrol",new EnemyPatrol());
            states.Add("Attack",new EnemyAttack());

            currentState = states["Patrol"];
            currentState.EnterState();
        }

        private void Update()
        {
            currentState.UpdateState();
        }

        void SwitchState(EnemyStateBase newState) {
            currentState.EnterState();
            newState.EnterState();
            currentState = newState;
        }
    }
}