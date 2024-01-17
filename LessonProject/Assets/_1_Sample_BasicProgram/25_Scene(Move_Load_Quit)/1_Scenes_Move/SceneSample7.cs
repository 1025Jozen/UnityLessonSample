using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample7 : MonoBehaviour
    {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.E)) {
                OnAccessGameManager();
            }
        }

        public void OnAccessGameManager() {
            //ロード済みのシーンであれば、名前で別シーンを取得できる
            //***DontDestroyOnLoadにはアクセスできない
            Scene scene = SceneManager.GetSceneByName("Scene_Sample3_Manager");

            //GetRootGameObjectsで、そのシーンのルートGameObjects
            //つまり、ヒエラルキーの最上位のオブジェクトが取得できる
            foreach (var rootGameObject in scene.GetRootGameObjects()) {
                GameManager gameManager = rootGameObject.GetComponent<GameManager>();
                //GetComponentInChildrenで子を拾う
                if (gameManager != null) {
                    //GameManagerが見つかったので
                    //gameManagerのスコアを取得
                    Debug.Log("スコアは" + gameManager.Score + "です");
                    break;
                }
            }
        }
    }
}

//private IEnumerator Start() {
//    yield return SceneManager.LoadSceneAsync("SceneB", LoadSceneMode.Additive);
//
//    ゲームオブジェクトを特定のシーンに移動させる例
//    Scene sceneB = SceneManager.GetSceneByName("SceneB");
//    SceneManager.MoveGameObjectToScene(m_objectB, sceneB);
//
//    yield return null;
//}