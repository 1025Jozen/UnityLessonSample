using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command
{
    public class PlayerMove :MonoBehaviour
    {
        public void MoveUp() {
            // 上に移動する処理を記述
            Debug.Log("Move Up");
            transform.position += transform.up * 2.0f *Time.deltaTime;
        }

        public void MoveDown() {
            // 下に移動する処理を記述
            Debug.Log("Move Down");
            transform.position -= transform.up * 2.0f * Time.deltaTime;
        }

        public void MoveLeft() {
            // 左に移動する処理を記述
            Debug.Log("Move Left");
            transform.position -= transform.right * 2.0f * Time.deltaTime;
        }

        public void MoveRight() {
            // 右に移動する処理を記述
            Debug.Log("Move Right");
            transform.position += transform.right * 2.0f * Time.deltaTime;
        }
    }
}


