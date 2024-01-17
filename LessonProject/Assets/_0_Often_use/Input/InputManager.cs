using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Often {
    public class InputManager : MonoBehaviour
    {
        InputSys inputSys;//PlayerInput使用
        public static InputManager Instance { get; private set; }//このクラス どこからでも参照可能

        public Vector3 inputVector { get; private set; }//InputManager.Instance.inputVectorで入力値を取得できる
        public Vector2 deltaMouse { get; private set; } 

        public event EventHandler OnFire;

        private void Awake() {
            if (Instance == null) {
                Instance = this;              
                inputSys = new InputSys();
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            inputSys.Player.Move.performed += Move;//pass through
            inputSys.Player.Look.performed += Look;

            inputSys.Player.Fire.started  += Fire;
            inputSys.Player.Fire.canceled += Fire;

            inputSys?.Enable();
        }
        private void OnDisable() {
            inputSys.Player.Move.performed -= Move;
            inputSys.Player.Look.performed -= Look;

            inputSys.Player.Fire.started  -= Fire;
            inputSys.Player.Fire.canceled -= Fire;

            inputSys?.Disable();
        }

        public void Move(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();   // Actionの入力値を読み込む
            inputVector = new Vector3(input.x, 0, input.y); // 入力値を保持しておく
        }

        public void Look(InputAction.CallbackContext context) {
            deltaMouse = context.ReadValue<Vector2>();   // Actionの入力値を読み込む
        }

        public void Fire(InputAction.CallbackContext context) {
            if (context.started) {
                //Debug.Log("ボタンを押した");

                //if (OnFire != null) {
                //    OnFire(this, EventArgs.Empty);
                //}

                //上を１行で書いただけ
                OnFire?.Invoke(this,EventArgs.Empty);
            } else if (context.canceled) {
                Debug.Log("ボタンを離した");
            }

           
        }

    }

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


//③
//Action Type = Pass Throughの場合
//基本的にデバイス入力がある間にperformedが呼ばれ続けます。
//デバイスが切り替わった場合は、切り替わり前のデバイスが無効（Disabled）となり、
//canceledコールバックが呼び出されます。



//Interactions
//Press
        //Press Only	押した瞬間のみperformedが呼ばれる
        //Release Only	離された瞬間のみperformedが呼ばれる
        //Press And Release	押した瞬間と離された瞬間両方でperformedが呼ばれる
//Hold
//Tap
//Slow Tap
//Multi Tap