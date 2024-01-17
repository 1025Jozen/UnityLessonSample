using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneSample
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) {
                OnAccessGameManger();
            }
        }

        void OnAccessGameManger() {
            //コンポーネント名.Instanceでシングルトンなコンポーネントを取得できる
            GameManager_Sample gameManager = GameManager_Sample.Instance;

            //GameManagerのスコアを表示
            Debug.Log(gameManager.Score);
        }
    }
}