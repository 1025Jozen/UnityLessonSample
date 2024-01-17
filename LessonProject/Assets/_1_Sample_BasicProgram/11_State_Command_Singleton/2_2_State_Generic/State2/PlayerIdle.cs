using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Generic1
{
    public class PlayerIdle : BaseState<PlayerStateMachine.PlayerState>
    {
        public PlayerIdle(PlayerStateMachine.PlayerState key) : base(key)
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
            if (Input.GetKeyDown(KeyCode.W))
                return PlayerStateMachine.PlayerState.Walk;
            else
                return PlayerStateMachine.PlayerState.Idle;
        }

        public override void ExitState() {

        }

        public override void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExit(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay(Collider other)
        {
            throw new System.NotImplementedException();
        }


    }
}