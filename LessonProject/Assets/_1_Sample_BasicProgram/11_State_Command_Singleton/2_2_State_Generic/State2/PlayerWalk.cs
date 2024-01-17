using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic1
{
    public class PlayerWalk : BaseState<PlayerStateMachine.PlayerState>
    {
        public PlayerWalk(PlayerStateMachine.PlayerState key) : base(key)
        {
        }

        public override void EnterState()
        {
            Debug.Log(StateKey);
        }

        //Idle状態でして欲しい挙動をEnterState UpdateState ExitStateに書く
        public override void UpdateState() {
            
        }

        //ステートチェック   //毎フレーム実行されてる
        //もし入力が検知されたら　別のステートをreturnするようにする
        public override PlayerStateMachine.PlayerState GetNextState()
        {
            if (Input.GetKeyDown(KeyCode.I))
              return PlayerStateMachine.PlayerState.Idle;
            else
              return PlayerStateMachine.PlayerState.Walk;
        }

        public override void ExitState() {

        }

        public override void OnTriggerEnter(Collider other)
        {
            
        }

        public override void OnTriggerExit(Collider other)
        {
            
        }

        public override void OnTriggerStay(Collider other)
        {
            
        }


    }
}