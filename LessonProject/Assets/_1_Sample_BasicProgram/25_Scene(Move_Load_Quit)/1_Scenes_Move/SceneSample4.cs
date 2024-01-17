using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample4 : MonoBehaviour
    {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                OnLoadSceneAdditive();
            }
        }
        public void OnLoadSceneAdditive() {
            //SceneBを加算ロード。現在のシーンは残ったままで、シーンBが追加される
            SceneManager.LoadScene("Scene_Sample2", LoadSceneMode.Additive);
        }
    }
}

