using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class PlayerGround : StateBase
    {
        public PlayerGround(StateFactory stateFactory, StateMachine stateMachine) : base(stateMachine, stateFactory) {
            isRootState = true;//ルートステート　特有の関数をインターフェイスとして継承させて見やすくするなどしてもいいかも
        }

        public override void EnterState()
        {
            Debug.Log("Gound ステートに入った");
            //ステートに入る時
            InitializeSubState();//サブステートを設定
        }
        public override void InitializeSubState() {
            if (InputSystem_Sample.PlayerMove1_ActionMap.Instance.inputVector.sqrMagnitude > 0.1f)
                SetChildState(Factory.Walk());
            else
                SetChildState(Factory.Idle());
        }

        public override void UpdateState(){
            //毎フレーム
            CheckSwitchStates();
        }
        public override void CheckSwitchStates() {
            //ステートの変更きっかけ
        }

        public override void ExitState() {
            //ステートを出る時
        }
    }
}