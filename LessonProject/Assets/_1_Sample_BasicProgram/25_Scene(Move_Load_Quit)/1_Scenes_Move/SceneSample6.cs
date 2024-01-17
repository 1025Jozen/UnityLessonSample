using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample6 : MonoBehaviour
    {
        private static bool Loaded { get; set; }

        //何らかのManeger(管理)に関するシーン
        void Awake() {
            if (Loaded) return;

            Loaded = true;
            SceneManager.LoadScene("Scene_Sample3_Manager", LoadSceneMode.Additive);
        }
    }
}

