using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleGame1
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] AllFruits allFruits;

        public int currentID { get; private set; } = 1; // 最初のID

        public static SpawnManager spawnManagerInstance;
        private void Start()
        {
            if (spawnManagerInstance == null) {
                spawnManagerInstance = this;
            } else {
                Destroy(this);
            }
        }

        private void Update()
        {
            if (Input.anyKeyDown) {
                SpawnPrefab();

                //すぐにボタンが押せないようにする
            }
        }

        public void SpawnPrefab() {
            // 新しいPrefabを生成 
            GameObject newPrefab = Instantiate(allFruits.fruitParamsList[0].fruitPrefab, Vector3.zero, Quaternion.identity);
            
            Fruit prefabScript = newPrefab.GetComponent<Fruit>();// 新しいPrefabにIDを割り当て
            if (prefabScript != null) {
                prefabScript.uniqueID = currentID;
            }

            // IDを増やして次のPrefabのために準備
            currentID++;
        }
    }
}



//// ゲームオブジェクトの生成と落下
//void SpawnRandomObject() {
//    float fallSpeed = 1f;
//    // ランダムなゲームオブジェクトを生成
//    GameObject newObject = Instantiate(RandomGameObjectPrefab());

//    // 初期位置や速度を設定し、ゲームオブジェクトを落とす
//    newObject.transform.position = SpawnPosition();
//    Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
//    rb.velocity = new Vector2(0, -fallSpeed);
//}

//GameObject RandomGameObjectPrefab() {
//    // ランダムなゲームオブジェクトプレハブを返す
//    // 適切な選択ロジックを実装

//    GameObject randomPrefab = new GameObject();
//    return randomPrefab;
//}

//Vector2 SpawnPosition() {
//    // 出現位置を計算する
//    // 箱内のランダムな位置を返す

//    Vector2 randomPosition = new Vector2(Random.Range(1, 10), Random.Range(1, 10));
//    return randomPosition;
//}