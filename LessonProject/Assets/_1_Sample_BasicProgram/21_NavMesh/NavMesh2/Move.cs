using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NavMesh_Sample
{
    public class Move : MonoBehaviour
    {
        public Transform goal;

        void Start() {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position;

            //ジャンプや落下の表現方法

            //キャラクターが指定範囲に入ったらagent.updatePositionをfalse
            //キャラクターの移動を（アニメーション・Tween・MatchTarget(Humanoidオンリー)・Timelineで）行う
            //移動完了後、agent.WarpでNavMeshAgentの位置を移動
            //agent.updatePositionをtrueで移動を再開
        }
    }
}