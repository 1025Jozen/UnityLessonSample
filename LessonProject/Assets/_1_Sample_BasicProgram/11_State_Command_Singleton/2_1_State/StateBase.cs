using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace State
{
    public abstract class StateBase 
    {
        protected StateFactory Factory { get; private set; }      //インスタンス生成クラスのアドレス保持
        protected StateMachine StateMachine { get; private set; } //状態遷移を実際に行っている場所       

        protected bool isRootState = false;                       //根本となるステートかどうか『 Grounded [ Run ][ Walk ][ Jump ]』でいうところのGroundedならtrue
        protected StateBase ParentState { get; private set; }     //親となるステートは何か
        protected StateBase ChildState  { get; private set; }     //サブステートは何か

        //コンストラクタ   //PlayerStateFactory で引数を割り当てる
        public StateBase( StateMachine stateMachine, StateFactory stateFactory) {
            Factory = stateFactory;      //各ステートのインスタンスアドレス情報が保存してあるクラス
            StateMachine = stateMachine; //使用中のstateMachineアドレスの入った変数。継承先でも使用可能         
        }

        //各ステートで設定する　       
        public abstract void EnterState();
        public abstract void InitializeSubState();
        public abstract void UpdateState();        //継承先のStateのここで毎フレームの処理を書く
        public abstract void CheckSwitchStates();
        public abstract void ExitState();

        /* 　   　　　　　　　　　　　　　　　　　　　　　　　　　　*/
        //ステート変更時　出た後、入る　。　　起動時は入っておく
        public void EnterStates() {
            EnterState();
            if (ChildState != null) {//サブステートが設定されているならサブステート実行
                ChildState.EnterStates();
            }
        }
        //Statemachine（本体）で回す
        public void UpdateStates() {
            UpdateState();
            if (ChildState != null) {//サブステートが設定されているならサブステート実行
                ChildState.UpdateStates();
            }
        }
        //ステート変更時　出てから入る
        protected void ExitStates() {
            ExitState();
            if (ChildState != null) {//サブステートが設定されているならサブステート実行
                ChildState.ExitStates();
            }
        }

        //サブステートを設定   InitializeSubState(　　)などでセット
        protected void SetChildState(StateBase newChildState) {
            ChildState = newChildState;
            ChildState.SetParentState(this);//サブステートに対して自分が親であることを設定
        }
        void SetParentState(StateBase newParentState) {
            ParentState = newParentState;
        }

        /* 　ステートを変更する際に呼び出す   */
        protected void SwitchState(StateBase newState) {//ステートが変わるとき呼び出す           
            if (newState.isRootState) {//大元のステートを変更するなら『ルートステート(親)　　「サブステート」「サブステート」』          
                ExitStates();                        //①現在の状態から抜け出し           
                newState.EnterStates();              //②新しい状態に
                StateMachine.CurrentState = newState;//③現在の状態をStateMachineクラスにて保持、Updateを回す
            } else if (StateMachine.CurrentState.ChildState != null) {//サブステートがあるなら　サブステートから抜け出すなどする
                StateMachine.CurrentState.ChildState.ExitStates();    //ステートを抜け出す
                StateMachine.CurrentState.SetChildState(newState);    //次のステートを親ステートにセット
                StateMachine.CurrentState.ChildState.EnterStates();   //サブステートに入る
            }
        }
    }
}
