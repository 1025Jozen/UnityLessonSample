using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEditorSamp
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] SampleRoot sampleRoot;

        private void Start() {

            Debug.Log(sampleRoot.labelText);
            //Debug.Log(sampleRoot.b);
            Debug.Log(sampleRoot.enemyName);
            Debug.Log(sampleRoot.hp);
            //Debug.Log(sampleRoot.power);
            //Debug.Log(SampleRoot.vec3);
            //Debug.Log(SampleRoot.animalKind);
        }
    }
}