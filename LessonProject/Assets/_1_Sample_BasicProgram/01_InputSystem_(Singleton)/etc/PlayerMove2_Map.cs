using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem_Sample
{
    //良し悪しは別にして　　一番　直感的にわかりやすいかも？
    public class PlayerMove2_Map : MonoBehaviour
    {
        ActionMap_Sample inputSystemMap;
        public static PlayerMove2_Map Instance { get; private set; }//どこからでも参照可能

        public Vector2 getMoveVec { get; private set; }
        public bool isAtackPressed { get; private set; }
        public bool isRunningPressed { get; private set; }
        public bool isJumpingPressed { get; private set; }

        /*   */
        private void Awake() {
                if (Instance != null) {
                    Debug.LogError("入力に関するインスタンスが無いよ");
                }
                Instance = this;
        }


        void Start() {
            inputSystemMap = new ActionMap_Sample();//設定されていたものをインスタンス化
        }
        private void OnEnable() {
            inputSystemMap.Enable();
        }
        private void OnDisable() {
            inputSystemMap.Disable();
        }





        /* 各種入力時の情報を取得  */
        public Vector2 GetMove() {//移動
            Vector2 inputVector = inputSystemMap.Player.Move.ReadValue<Vector2>();
            return inputVector;
        }

        public bool Attack() {//攻撃ボタンを押した瞬間
            return inputSystemMap.Player.Attack.WasPressedThisFrame();
        }


        public bool OnRun() {
            isRunningPressed = inputSystemMap.Player.Run.IsPressed();//入力受付
            return isRunningPressed;
        }

        public bool OnJump() {
            isJumpingPressed = inputSystemMap.Player.Jump.IsPressed();
            return isJumpingPressed;
        }




        //private Vector2 m_currentMousePosition;
        private PlayerInput m_playerInput;

        // マウス座標が更新された時に通知するコールバック関数
        public void OnMousePosition(InputAction.CallbackContext _context) {
            //m_currentMousePosition = _context.ReadValue<Vector2>();
        }
        void Update() {
            //var worldPosition = Camera.main.ScreenToWorldPoint(m_currentMousePosition);
            var mousePosition = inputSystemMap.Player.Look.ReadValue<Vector2>();
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.Log(worldPosition);
        }




    }
}