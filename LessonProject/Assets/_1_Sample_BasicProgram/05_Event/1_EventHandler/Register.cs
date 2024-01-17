using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSample {
    public class Register : MonoBehaviour
    {
        [SerializeField] float enemy_hp = 10;
        [SerializeField] float player_hp = 10;
        [SerializeField] float enemy_attack = 5;

        private void Start()
        {
            Root_Invoke.Instance.OnDamage += Damage;
            //Root_Invoke.Instance.OnDamage += パーティクル生成関数;

            Root_Invoke.Instance.OnAttack += Attack;         
        }

        private void Damage(object sender, EventArgs e) {
            player_hp -= enemy_attack;

            Debug.Log($"Player_Hp : {player_hp}");
        }

        private void Attack(object sender, Root_Invoke.AttackParam e)
        {
            enemy_hp -= e.power;

            Debug.Log($"Enemy_Hp : {enemy_hp}");
        }

    }
}