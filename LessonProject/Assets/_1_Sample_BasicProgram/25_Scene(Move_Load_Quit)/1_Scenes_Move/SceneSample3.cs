using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample3 : MonoBehaviour
    {
        private static bool Loaded { get; set; }

        [SerializeField] GameObject[] gameManagerPrefabs = null;

        void Awake() {
            if (Loaded) return;//すでにロード済みなら、二重に作成しない

            Loaded = true;

            //プレハブをインスタンス化して、DontDestroyOnLoad
            foreach (var prefab in gameManagerPrefabs) {
                GameObject gO = Instantiate(prefab);
                DontDestroyOnLoad(gO);
            }
        }
    }
}

