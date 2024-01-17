using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LoadScene_Sample
{
    public class LoadCallback : MonoBehaviour
    {
        bool isFirstUpdate = true;
        void Update() {
            if (isFirstUpdate) {
                isFirstUpdate = false;
                Load.LoadCallback();
            }
        }
    }
}