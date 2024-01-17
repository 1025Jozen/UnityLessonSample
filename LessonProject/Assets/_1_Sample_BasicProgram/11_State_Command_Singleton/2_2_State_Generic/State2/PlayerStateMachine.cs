using System.Collections;
using System.Collections.Generic;
using State_Generic;
using UnityEngine;


namespace State_Generic1
{
    //Playerを動かすときの大元
    public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState> {
        public Animation anim{ get; set; }

        public enum PlayerState {
            Idle,
            Walk,
            Run
        }

        private void Awake()
        {
            states.Add(PlayerState.Idle, new PlayerIdle(PlayerState.Idle));
            states.Add(PlayerState.Walk, new PlayerWalk(PlayerState.Walk));

            currentState = states[PlayerState.Idle];
        }
    }
}