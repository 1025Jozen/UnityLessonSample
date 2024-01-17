using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace AsyncSample {
    public class SampleUniTask : MonoBehaviour
    {

        async void Test() {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }
        
    }
}

