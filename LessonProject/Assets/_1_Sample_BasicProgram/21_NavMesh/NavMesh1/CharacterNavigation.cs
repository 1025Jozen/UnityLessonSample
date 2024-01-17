using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavMesh_Sample {
    public class CharacterNavigation : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Target;

        private UnityEngine.AI.NavMeshAgent m_Agent;

        void Start() {
            m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        void Update() {
            m_Agent.SetDestination(m_Target.position);
        }
    }
}

