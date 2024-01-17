using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace BLINDED_AM_ME {
    public class MeshCut_Blade : MonoBehaviour
    {
        [SerializeField] Material mat;
        [SerializeField] Transform plane_Point;
        [SerializeField] Vector3 normalVect;
        void Start() {

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) {

                Debug.Log("当たった");
                MeshCut.Cut(hit.collider.gameObject, plane_Point.position, normalVect, mat);

            }
        }
    }
}

