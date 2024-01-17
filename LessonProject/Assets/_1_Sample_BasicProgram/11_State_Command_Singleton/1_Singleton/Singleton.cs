using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Singleton_Sample {
    public class SingletonSample : MonoBehaviour
    {
        public static SingletonSample instance;
        int count;

        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        /*               */
        public void test0() {            
            count++;
            Debug.Log($"テスト0 : {count}");
        }

        public void test1(out int t) {
            t = count;
            Debug.Log($"テスト1 : {t}");
        }

        public void test2(int t) {
            t = count;
            Debug.Log($"テスト2 : {t}");
        }


    }
}

