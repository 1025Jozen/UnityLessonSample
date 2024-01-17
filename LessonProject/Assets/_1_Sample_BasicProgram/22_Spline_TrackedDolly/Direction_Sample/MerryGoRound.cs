using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace P5 {
    public class MerryGoRound : MonoBehaviour
    {
        CinemachineDollyCart cartPosition;
        [SerializeField] float speed;

        void Start() {
            cartPosition = GetComponent<CinemachineDollyCart>();
        }


        private void LateUpdate()
        {
            cartPosition.m_Position += speed * Time.deltaTime;
        }
    }
}