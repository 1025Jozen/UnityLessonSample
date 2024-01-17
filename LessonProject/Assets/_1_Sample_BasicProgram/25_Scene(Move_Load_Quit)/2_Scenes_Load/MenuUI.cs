#define UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LoadScene_Sample {
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] Button playButton;
        [SerializeField] Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(() => {
                //SceneManager.LoadScene(7);//直接じゃなくてワンクッションはさむ

                Load.LoadScene(Load.Scene.Scene2_Game);//Loadシーンに遷移(１度Loadシーンを読んでからGameSceneへ)
            });
            quitButton.onClick.AddListener(() => {
                //ゲームプレイ終了
                #if UNITY_EDITOR //UnityEditorで作用
                    UnityEditor.EditorApplication.isPlaying = false;
                #else //ビルドしたらこっち
                    Application.Quit();
                #endif
            });
        }
    }
}
