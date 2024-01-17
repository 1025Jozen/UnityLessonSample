using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glass {
    public class CameraRotate : MonoBehaviour
    {
        Vector3 rotation;
        [SerializeField] float rotationSpeed = 10f;

        private void Awake()
        {
            rotation = transform.eulerAngles;
        }

        void Update() {
            Vector2 delta = Often.InputManager.Instance.deltaMouse;

            rotation = rotation + new Vector3(-delta.y, delta.x, 0) * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}

