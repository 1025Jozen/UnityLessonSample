using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Player : MonoBehaviour
    {
        float currentHp;
        float maxHp = 10f;

        public event EventHandler<HpEventArgs> HpChanged;
        public class HpEventArgs : EventArgs {
            public float hpNormalized;
        }

        private void Start()
        {
            currentHp = maxHp;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) {
                currentHp -= 2;
                //Hpが変更された際のイベントを(登録されていれば)実行
                HpChanged?.Invoke(this, new HpEventArgs {
                    hpNormalized = (float)currentHp / maxHp
                }) ;
            }
        }
    }
}

