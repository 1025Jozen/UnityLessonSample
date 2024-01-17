using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem_Sample
{
    //入力受付方法

    //「Player Input」参照方法は
    //・InputActionをシングルトンクラスで管理する
    //・ScriptableObjectを使用
    //・デリゲートやイベントを使用
    //などあるかもしれないが、ここでは「シングルトンクラスで管理する」

    public class PlayerMove1_ActionMap : MonoBehaviour
    {
        ActionMap_Sample input;//「PlayerInput」コンポーネントからC#のクラス　スクリプトを作成
        public static PlayerMove1_ActionMap Instance { get; private set; }//どこからでも参照可能


        public event EventHandler FireAction;
       
        public Vector3 inputVector { get; private set; }


        private void Awake()
        {
            if (Instance == null) {
                Instance = this;               //このクラスのインスタンスアドレスをstaticな変数に保存
                input = new ActionMap_Sample();//「PlayerInput」コンポーネントからC#のクラス　スクリプトを作成しインスタンス化
            } else {
                Destroy(gameObject);
            }
        }


        //デリゲート
        private void OnEnable() {        
            input.Player.Move.performed += Move;           //pass through(下記③)
            input.Player.Fire.started += OnFirePerformed;

            input?.Enable();
        }

        

        private void OnDisable() {
            input.Player.Move.performed -= Move;
            input.Player.Fire.started -= OnFirePerformed;

            input?.Disable();
        }





        //started   – 入力され始めた時などに呼ばれる
        //performed – 特定の入力があった時などに呼ばれる
        //canceled  – 入力が中断された時などに呼ばれる

        //①
        //Action Type = Valueの場合
        //started   – 入力が0から0以外に変化したとき
        //performed – 入力が0以外に変化したとき
        //canceled  – 入力が0以外から0に変化したとき


        //②
        //Action Type = Buttonの場合
        //started   – 入力が0から0以外に変化したとき
        //performed – 入力の大きさが閾値Press以上に変化したとき
        //canceled  – 入力が0以外から0に変化したとき、またはperformedが呼ばれた後に入力の大きさが閾値Release以下に変化したとき

        //閾値の設定は、トップメニューのEdit > Project Settings > Input System Packageの項目から変更できます。
        //閾値Pressの値はDefault Press Button Point、
        //閾値Releaseの値は「Press × Button Release Threshold」となります。閾値ReleaseはPressと掛け算した値であることに注意する必要があります

        //③
        //Action Type = Pass Throughの場合
        //基本的にデバイス入力がある間にperformedが呼ばれ続けます。
        //デバイスが切り替わった場合は、切り替わり前のデバイスが無効（Disabled）となり、
        //canceledコールバックが呼び出されます。






        /*    スクリプト２種    */

        //「On~」でActionMapに設定した名前を使用   +=　などの設定はいらない
        //public void OnMove(InputValue movementValue) {
        //    Vector2 input = movementValue.Get<Vector2>();     // Actionの入力値を読み込む
        //    inputVector = new Vector3(input.x , 0 , input.y); // 入力値を保持しておく
        //}


        //上のように「On~」でActionMapに設定した名前を使用　する必要はないが　+=　で関数を設定
        public void Move(InputAction.CallbackContext context) {
            //if (context.action.name != "Move") return;    // Move以外は処理しない
            Vector2 input = context.ReadValue<Vector2>();   // Actionの入力値を読み込む
            inputVector   = new Vector3(input.x, 0, input.y); // 入力値を保持しておく
        }


        public void OnFirePerformed(InputAction.CallbackContext context) {
            //ボタンが押されたらFireActionに登録されたアクションが実行される
            FireAction?.Invoke(this,EventArgs.Empty);
        }
    }

}

