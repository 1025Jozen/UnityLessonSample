using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoadScene_Sample
{
    public static class Load
    {
        private static Scene targetScene;

        public enum Scene {
            Scene1_Menu,
            Scene2_Game,
            Scene3_Loading
        }

        //ここはロードシーンを呼び出すためのstaticなクラスのメソッド(遷移前のシーンから呼び出される)
        public static void LoadScene(Scene targetScene) {
            Load.targetScene = targetScene;
            SceneManager.LoadScene(Scene.Scene3_Loading.ToString());
        }

        //ロードシーンから呼び出される
        public static void LoadCallback() {
            SceneManager.LoadScene(targetScene.ToString());
        }
    }
}
