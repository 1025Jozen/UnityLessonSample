using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace State_Generic1
{
    //UIを動かすとき
    public class  UIStateMachine: StateManager<UIStateMachine.UIState>
    {
        public enum UIState {
            Menu,
            Loading,
            Options
        }
    }
}