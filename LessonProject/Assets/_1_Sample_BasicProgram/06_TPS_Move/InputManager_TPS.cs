using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TPS
{
    public class InputManager_TPS : MonoBehaviour
    {
        ActionMap_Sample input;//「PlayerInput」コンポーネントからC#のクラス　スクリプトを作成
        public static InputManager_TPS Instance { get; private set; }//どこからでも参照可能
       
        public Vector3 inputMoveVector { get; private set; }
        public Vector2 inputLookVector { get; private set; }
        public bool pushingRun { get; private set; }

        private void Awake()
        {
            if (Instance == null) {              
                input = new ActionMap_Sample();//「PlayerInput」コンポーネントからC#のクラス　スクリプトを作成しインスタンス化
                Instance = this;               //このクラスのインスタンスアドレスをstaticな変数に保存
            } else {
                Destroy(gameObject);
            }
        }

        private void OnEnable() {        
            input.Player.Move.performed += Move;//pass through
            input.Player.Look.performed += Look;
            input?.Enable();
        }
        private void OnDisable() {
            input.Player.Move.performed -= Move;
            input.Player.Look.performed -= Look;
            input?.Disable();
        }

        public void Move(InputAction.CallbackContext context) {
            if (context.action.name != "Move") return;    // Move以外は処理しない
            Vector2 input = context.ReadValue<Vector2>();   // Actionの入力値を読み込む
            inputMoveVector = new Vector3(input.x, 0, input.y); // 入力値を保持しておく

            //Debug.Log(inputMoveVector);
        }
        public void Look(InputAction.CallbackContext context) {
            inputLookVector = context.ReadValue<Vector2>(); //入力ベクトルを読み取る
        }
        public bool Run() {
            return pushingRun = input.Player.Run.IsPressed();
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






/*    スクリプト２種    */

//「On~」でActionMapに設定した名前を使用   +=　などの設定はいらない
//public void OnMove(InputValue movementValue) {
//    Vector2 input = movementValue.Get<Vector2>();     // Actionの入力値を読み込む
//    inputVector = new Vector3(input.x , 0 , input.y); // 入力値を保持しておく
//}
