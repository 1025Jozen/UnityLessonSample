using UnityEngine;

namespace VContainer1
{
    /// <summary>ゲーム管理クラス</summary>
    public class GameManager
    {
        public void OnStart() {
            Debug.Log("Start");
        }
        public void OnUpdate() {
            Debug.Log("Update");
        }
        public void Hello() {
            Debug.Log("Hello world");
        }
    }
}