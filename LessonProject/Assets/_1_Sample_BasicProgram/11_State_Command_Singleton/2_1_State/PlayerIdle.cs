using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace State
{

    public class PlayerIdle : StateBase
    {
        //コンストラクタ 　引数を扱えるようにする
        public PlayerIdle(StateFactory stateFactory, StateMachine stateMachine) : base(stateMachine, stateFactory) {
        }
        public override void EnterState() {
            Debug.Log("Idle状態");
        }
        public override void InitializeSubState() {
           
        }

        public override void UpdateState() {
            CheckSwitchStates();
        }
        public override void CheckSwitchStates() {
            if (InputSystem_Sample.PlayerMove1_ActionMap.Instance.inputVector.sqrMagnitude > 0.1f)
                SwitchState(Factory.Walk());
        }

        public override void ExitState() {
            
        }
    }

}