//シングルトンなMonoBehaviourの基底クラス
using UnityEngine;

namespace SceneSample
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (T)FindFirstObjectByType(typeof(T));

                if (instance == null) {
                    Debug.LogError(typeof(T) + "is nothing");
                }

                return instance;
            }
        }

        public static T InstanceNullable
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance != null && instance != this) {
                Debug.LogError(typeof(T) + " is multiple created", this);
                return;
            }

            instance = this as T;
        }

        protected virtual void OnDestroy() {
            if (instance == this) {
                instance = null;
                //一度GameObjectとしてOnになっていないとOnDestroyメソッドは呼ばれない
                //参照元を消してリークしないように これがないと見た目上消えていてもない内部的に残っている
            }
        }
    }
}