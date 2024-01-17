using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample1 : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) {
                OnLoadSceneSingle();
            }
        }

        public void OnLoadSceneSingle() {
            //Scene_Sample2をロード。現在のシーンは自動的に削除されて、Scene_Sample2だけになる
            //自動的に前の状態も保存されない
            SceneManager.LoadScene("Scene_Sample2");
        }
    }
}

