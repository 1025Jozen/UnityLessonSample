using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Often
{
    public class CursorCtrl : MonoBehaviour
    {
        void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            if (Input.GetMouseButtonDown(0)) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}