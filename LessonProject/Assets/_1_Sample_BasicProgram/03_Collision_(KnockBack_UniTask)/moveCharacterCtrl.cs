using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnockBack
{
    public class moveCharacterCtrl : MonoBehaviour
    {
        CharacterController characterController;

        void Start() {
            characterController = GetComponent<CharacterController>();
        }

        void Update() {
            Vector3 playerVelocity = new Vector3(1f, 0f, 0f);
            characterController.Move(playerVelocity * Time.deltaTime);
        }
    }
}