using UnityEngine;

namespace Value_and_Reference1{
    public class Sample2 : MonoBehaviour
    {
        Vector3 v;

        void Start() {
            v = transform.position;
        }
        void Update() {
            v += Vector3.right;
        }
    }
}

