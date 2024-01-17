using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem_Sample
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        CharacterController controller;
       
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            PlayerMove1_ActionMap.Instance.FireAction += Instance_FireAction;
        }

        private void Update()
        {
            controller.Move(PlayerMove1_ActionMap.Instance.inputVector * speed * Time.deltaTime);
        }

        private void Instance_FireAction(object sender, System.EventArgs e) {
            Debug.Log("登録されたメソッド実行");
        }
    }
}