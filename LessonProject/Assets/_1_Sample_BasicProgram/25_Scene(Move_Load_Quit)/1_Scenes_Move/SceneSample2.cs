using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneSample {
    public class SceneSample2 : MonoBehaviour
    {
        void Awake() {   
            //DontDestroyシーンに移る
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

