using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonSkyBox {
    [ExecuteInEditMode]
    public class GetMainLightDirection : MonoBehaviour
    {
        [SerializeField] Material skyBoxMaterial;

        void Start() {

        }

        void Update() {
            skyBoxMaterial.SetVector("_MainLightDirection" , transform.forward);
            skyBoxMaterial.SetVector("_MainLightUp", transform.up);
            skyBoxMaterial.SetVector("_MainLightRight", transform.right);
        }
    }
}

