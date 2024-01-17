using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interval {
    public class Interval1 : MonoBehaviour
    {
        [SerializeField] float interval　= 3;//間隔

        private void Start() {
            StartCoroutine("MainLoop");
        }

        IEnumerator MainLoop() {
            while (true) {
                yield return new WaitForSeconds(interval);// interval 秒待つ

                Debug.Log($"定期的に実行: \n" +
                    $"現在時間 = {DateTime.Now}\n " +
                    $"起動後の経過時間 = {Time.realtimeSinceStartup}\n" +
                    $"起動後のフレーム数 = {Time.frameCount}\n"
                );
            }
        }
    }
}

