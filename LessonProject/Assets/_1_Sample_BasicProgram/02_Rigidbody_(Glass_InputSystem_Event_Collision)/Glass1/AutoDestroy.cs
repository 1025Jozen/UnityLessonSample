using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glass {
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] float timeToLive = 1f;

        protected virtual void Awake() {
            Destroy(gameObject, timeToLive);
        }
    }
}

