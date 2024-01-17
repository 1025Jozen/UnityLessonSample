using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem_Sample {

    //ボタンをインスペクターで設定する　(Actionの設定)
    public class PlayerMove3_Action : MonoBehaviour
    {
        ActionMap_Sample input;


        [SerializeField]InputAction inputAction; //Player の　Move　にあたる
         
        private void Awake()
        {
            //input = new ActionMap_Sample();    
        }

        private void OnEnable() {
            inputAction?.Enable();
            inputAction.performed += InputAction_performed;
        }
        private void OnDisable() {
            input?.Disable();
            inputAction.performed -= InputAction_performed;
        }

        private void InputAction_performed(InputAction.CallbackContext obj)
        {
            // Actionの入力値を読み込む
            var value = obj.ReadValue<Vector2>();

            // 入力値をログ出力
            Debug.Log($"Actionの入力値 : {value}");
        }



        void Start() {

        }


        void Update() {
 
        }
    }

}

