using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SampleGame1 {
    public class Fruit : MonoBehaviour
    {
        [SerializeField] AllFruits allFruits;
        [NonSerialized] public float uniqueID; // Spawn時にcurrentIdを設定

        //生成された時の　点数を保持　後でGameManagerに点数を送る

        string fruitName;

        private void Start()
        {
            uniqueID = SpawnManager.spawnManagerInstance.currentID;
        }

        void OnCollisionEnter(Collision collision) {            
            GameObject otherObject = collision.gameObject;// 衝突したゲームオブジェクトを取得
            if ( otherObject.TryGetComponent<Fruit>(out var col)) {
                if (uniqueID < col.uniqueID) {// IDが小さい方を基準にする（後で位置の平均計算するけど）
                                              // 「同じ名前なら」も後で追加
                    // 2つのゲームオブジェクトを削除
                    Destroy(gameObject);
                    Destroy(otherObject);

                    // 新しいゲームオブジェクトを生成
                    Vector3 newPosition = CalculateNewPosition(transform.position, otherObject.transform.position);
                    GameObject newObject = Instantiate(allFruits.fruitParamsList[1].fruitPrefab, newPosition, Quaternion.identity);
                }

            }

        }

        Vector3 CalculateNewPosition(Vector3 position1, Vector3 position2) {
            // 新しいゲームオブジェクトの位置を計算するロジックを実装
            // 今回は 2 つ の ゲーム オブジェクト の 中間 
            return (position1 + position2) / 2;
        }
    }
}

