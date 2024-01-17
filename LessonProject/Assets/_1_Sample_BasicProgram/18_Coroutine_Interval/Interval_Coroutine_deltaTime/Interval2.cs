using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Interval {
    public class Interval2 : MonoBehaviour
    {
        public bool LoopActive;
        [SerializeField] float interval;//間隔
        [SerializeField] UnityEvent doSomething;

        float interval_cnt;//現在の経過時間(残り時間)

        void Update() {
            if (LoopActive) {
                if (interval_cnt <= 0) {//残り時間が0になったら何かしら実行
                    doSomething?.Invoke();
                    interval_cnt = interval;
                }
                if (interval_cnt > 0)
                    interval_cnt -= Time.deltaTime;
            }
        }
    }
}

