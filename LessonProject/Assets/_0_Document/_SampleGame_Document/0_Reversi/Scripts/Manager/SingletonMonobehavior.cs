using UnityEngine;
using System;

namespace Othello
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null) {
                    Type t = typeof(T);
                    instance = (T)FindFirstObjectByType(t);//全オブジェクトを探索,名前が一致したらオブジェクト取得
                    if (instance == null)
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
                return instance;
            }
        }

        virtual protected void Awake() {
            CheckInstance();// 他のゲームオブジェクトにアタッチされているか調べる// アタッチされている場合は破棄する。
            DontDestroyOnLoad(this.gameObject);
        }

        protected bool CheckInstance() {
            if (instance == null) {
                instance = this as T;
                return true;
            } else if (Instance == this) {
                return true;
            }
            Destroy(this.gameObject);
            return false;
        }
    }
}