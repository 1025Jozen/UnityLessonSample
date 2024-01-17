using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace State_Generic
{
    public abstract class PlayerStateBase 
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}
