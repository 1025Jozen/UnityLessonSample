using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSample {
    public class Root_Invoke : MonoBehaviour {
        [SerializeField] KeyCode attackKey = KeyCode.A;
        [SerializeField] KeyCode damageKey = KeyCode.D;

        public static Root_Invoke Instance { get; private set; }



        public event EventHandler<AttackParam> OnAttack;
        public class AttackParam : EventArgs
        {
            public float power;
        }

        public event EventHandler OnDamage;



        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }
        }

        


        void Update() {
            if (Input.GetKeyDown(attackKey)) {

                //敵のHPを減らす  player_Attack

                OnAttack?.Invoke(this, new AttackParam { power = 1 });//イベント実行 //攻撃イベントを呼び出す
            }

            if (Input.GetKeyDown(damageKey)) {

                //敵に当たったら  playerのダメージ処理を行う

                OnDamage?.Invoke(this,EventArgs.Empty);
            }

        }


    }
}