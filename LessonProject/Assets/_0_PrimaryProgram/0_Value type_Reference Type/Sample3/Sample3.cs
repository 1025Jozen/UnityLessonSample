using System;
using System.Collections.Generic;
using UnityEngine;

namespace Value_and_Reference3
{
    public class Sample3 : MonoBehaviour
    {
        [SerializeField] BoxCollider box;

        private void Start()
        {
            //Debug.Log(box);
            //box = GetComponent<BoxCollider>();
            Debug.Log(box);

            Vector3 v = new Vector3();
            v.x = 10;
            v.y = 5;

            box.size = v;
        }
    }
}