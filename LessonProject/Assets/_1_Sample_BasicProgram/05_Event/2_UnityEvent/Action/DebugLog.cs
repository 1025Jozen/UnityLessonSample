using UnityEngine;


namespace UnityEvent_Sample
{
    public class DebugLog : MonoBehaviour
    {
        [SerializeField] OnCollisionEnterEvent onCollisionEnter;

        private void Start() {            
            onCollisionEnter.collisionAction += View;// collisionAction に  関数を登録（引数が一致している必要がある）
        }

        private void View(Collision collision) {
            Debug.Log("Action : " + collision.gameObject.name);
        }

        public void UnityEvent_View(Collision collision) {
            Debug.Log("UnityEvent : " + collision.gameObject.name );
        }
    }
}