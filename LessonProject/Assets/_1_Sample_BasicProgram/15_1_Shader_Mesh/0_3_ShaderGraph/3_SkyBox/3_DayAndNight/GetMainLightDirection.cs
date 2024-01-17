using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyBox {
    [ExecuteInEditMode]
    public class GetMainLightDirection : MonoBehaviour
    {
        [SerializeField] Material skyBoxMaterial;

        void Start() {

        }

        void Update() {
            skyBoxMaterial.SetVector(name = "_MainLightDirection" , transform.forward);
        }
    }
}

