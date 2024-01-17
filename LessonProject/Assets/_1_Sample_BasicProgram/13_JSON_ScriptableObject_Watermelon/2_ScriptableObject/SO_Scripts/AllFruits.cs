using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleGame1
{
    [CreateAssetMenu(fileName = "AllFruits", menuName = "SampleGame1/AllFruits")]
    public class AllFruits : ScriptableObject
    {
        //辞書はシリアライズ化できない
        //public Dictionary<int, FruitParam> fuitDictionary = new Dictionary<int, FruitParam>();

        public　List<FruitParam>  fruitParamsList = new List<FruitParam>();

    }
}