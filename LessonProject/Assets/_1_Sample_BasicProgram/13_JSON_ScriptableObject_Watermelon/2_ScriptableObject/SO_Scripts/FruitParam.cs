using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleGame1
{
    [CreateAssetMenu(fileName = "FruitParam", menuName = "SampleGame1/FruitParam")]
    public class FruitParam : ScriptableObject
    {
        public string fruitname;
        public FruitState fruitState;
        public GameObject fruitPrefab;                  
    }

    [System.Serializable]
    public enum FruitState
    {
        Cherry,
        Strawberry
    }
}