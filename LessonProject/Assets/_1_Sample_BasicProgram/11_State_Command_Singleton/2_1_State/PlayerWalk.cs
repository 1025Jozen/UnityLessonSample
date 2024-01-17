using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace State
{
    public class PlayerWalk : StateBase
    {
        //コンストラクタ 　引数を扱えるようにする
        public PlayerWalk(StateFactory stateFactory,StateMachine stateMachine)
            : base(stateMachine, stateFactory) {

        }

        float walkspeed = 2.0f;

        public override void EnterState() {
            Debug.Log("Walk状態");
        }
        public override void InitializeSubState() {
        }

        public override void UpdateState() {
            StateMachine.controller.Move(InputSystem_Sample.PlayerMove1_ActionMap.Instance.inputVector * walkspeed * Time.deltaTime);
            
            CheckSwitchStates();
        }
        public override void CheckSwitchStates() {
            if (InputSystem_Sample.PlayerMove1_ActionMap.Instance.inputVector.sqrMagnitude < 0.1f)
                SwitchState(Factory.Idle());
        }

        public override void ExitState() {

        }







        ///*  */
        //public override void EnterState() {
        //    Debug.Log("Enter Walk State");
        //    State.StateMachine.Animator.SetBool(State.StateMachine.isWalking_Hash, true);
        //    InitializeSubState();
        //}

        //public override void InitializeSubState() {
        //    SetParentState(Factory.Grounded());
        //}

        //public override void UpdateState() {

        //    //StateMachine.GetInput();　　　　　  //入力量を取得
        //    //StateMachine.PlayerMoveDirection();//カメラから動かしたい方向を把握                                   
        //    //StateMachine.PlayerRotation();     //回転
        //    //HandleGravity();                   //重力 としてplayerVelocity.yの値を変更

        //    CheckSwitchStates();
        //}

        //public override void ExitState() {
        //    State.StateMachine.Animator.SetBool(State.StateMachine.isWalking_Hash, false);
        //}

        //public override void CheckSwitchStates() {
        //    if (State.StateMachine.inputMagnitude < State.StateMachine.allowMoveMagnitude) {//棒立ち
        //        SwitchState(Factory.Idle());
        //    } else if (State.StateMachine.inputMagnitude > State.StateMachine.allowMoveMagnitude && State.StateMachine.InputManager.OnRun()) {
        //        SwitchState(Factory.Run());
        //    }
        //}


        ///* */
        ////重力
        //void HandleGravity() {
        //    bool isFalling = State.StateMachine.playerVelocity.y <= 0.0f;//ジャンプボタンを押していなかったら落ちる判定にして重力を強くする
        //    float fallMultiplier = 2.0f;//落ちる時は早めに落ちるように
        //    if (State.StateMachine.CollisionResults.IsGrounded) {//地面についてる時
        //        State.StateMachine.playerVelocity.y = State.StateMachine.groundedGravity;
        //    } else if (isFalling) {//落下中
        //        State.StateMachine.playerVelocity.y += Mathf.Max((State.StateMachine.gravity * fallMultiplier * Time.deltaTime), State.StateMachine.fallingMaxSpeed);//落下中は早めに落とす 速度制限ももうけてみた
        //    } else {//それ以外（上昇中など）
        //        State.StateMachine.playerVelocity.y += State.StateMachine.gravity * Time.deltaTime;// 通常通り重力を適用
        //    }
        //}

        //void HandleAnimation() {
        //    State.StateMachine.Animator.SetBool("isWalking", false);
        //}
    }
}