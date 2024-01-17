using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Value_and_Reference1
{
    public class Sample1 : MonoBehaviour
    {
        Transform t;

        void Start() {
            t = transform;
        }
        void Update() {
            t.position += Vector3.right;
        }
    }
}

