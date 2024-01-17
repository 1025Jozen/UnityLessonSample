using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetLock
{
    public class Player : MonoBehaviour
    {
        public List<Transform> targets = new List<Transform>();
        Transform target;

        void Start() {

        }

        void Update() {
            //可読性悪いから変えるかも　インデックス番号がnullじゃないならインデックス番号目の要素を　nullならnullを入れる
            target = targetIndex() != null ? targets[targetIndex() ?? 0] : null;

            if (target) {
                Debug.Log($"{ target }を狙う。{ target?.position }の位置に関して操作");                
            }
        }


        public int? targetIndex() {
            float[] distances = new float[targets.Count];//ターゲットの数ぶんの配列

            for (int i = 0; i < targets.Count; i++) {
                //左下(0,0)  右上(1,1)          ビューポートポイント Camera.main.WorldToViewportPoint(transform座標)
                //左下(0,0)  右上(1920,1080)など スクリーンポイント  Camera.main.WorldToScreenPoint(transform座標)
                distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(targets[i].position), new Vector2(Screen.width / 2, Screen.height / 2));
            }

            float minDistance = Mathf.Min(distances);//一番近くまでの長さ
            int? index = null;

            for (int i = 0; i < distances.Length; i++) {
                if (minDistance == distances[i]) index = i;//「一番近くまでの長さ」は配列の何番目で該当するか
            }

            return index;

        }
    }
}