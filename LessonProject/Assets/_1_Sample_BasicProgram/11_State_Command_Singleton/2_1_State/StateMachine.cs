using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    //このスクリプトをアタッチしてキャラを動かす　　　　//ここが大元
    public class StateMachine:MonoBehaviour
    {
        /*  　　　変数 　　　*/
        StateFactory states;                       //各ステートのインスタンスの生成先 //各ステートのインスタンスを保持しているところ       
        public StateBase CurrentState { get; set; }//ステート保存先 //現在のステートを保持・操作するステート      

        
        public CharacterController controller { get; private set; }

        private void Awake()
        {
            states     = new StateFactory(this); //ステート作成・インスタンス作成//各ステートのインスタンス化と辞書設定　を実行
            controller = GetComponent<CharacterController>();
        }


        private void Start()
        {
            CurrentState = states.Ground();//初期ステート設定 棒立ち
            CurrentState.EnterStates();
        }

        private void Update()
        {
            CurrentState.UpdateStates();
        }

    }
}