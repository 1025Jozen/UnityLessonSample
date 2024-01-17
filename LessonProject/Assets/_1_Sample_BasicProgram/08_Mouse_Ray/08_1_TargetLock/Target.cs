using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetLock {
    public class Target : MonoBehaviour
    {
        Player player;
        void Start() {
            player = FindFirstObjectByType<Player>();
        }

        private void OnBecameVisible() {
            if (!player.targets.Contains(transform))
                player.targets.Add(transform);
        }

        private void OnBecameInvisible() {
            if (player.targets.Contains(transform))
                player.targets.Remove(transform);
        }
    }
}

