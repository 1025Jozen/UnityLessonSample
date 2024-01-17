using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace State_Generic1
{
    //Enemyを動かすとき
    public class  EnemyStateMachine: StateManager<EnemyStateMachine.EnemyState>
    {
        public enum EnemyState
        {
            Patrol,
            Attack,
            Die
        }
    }
}