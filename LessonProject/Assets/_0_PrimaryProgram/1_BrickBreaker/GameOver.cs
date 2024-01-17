using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace BrickBreaker_Sample
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gameOverMessage; //Inspectorから設定できるようにする

        bool isGameOver = false;// ゲームオーバーしたかどうかを判断するための変数                     
        void Update() {
            // ゲームオーバーになっている、かつ、Submitボタンを押したら実行する
            if (isGameOver && Input.GetButtonDown("Submit")) {
                SceneManager.LoadScene("BrickBreaker");// Playシーンをロードする　
            }
        }


        void OnCollisionEnter(Collision collision) {// 衝突時に呼ばれる
            if (collision.transform.TryGetComponent<Ball>(out Ball ball)) {
                gameOverMessage.text = "Game Over\n<size=50>~Press Enter~</size>";// Game Overと表示する
                Destroy(collision.gameObject);     // 当たったゲームオブジェクトを削除する
                isGameOver = true;                 // isGameOverをtrueにする（フラグを立てる）
            }
        }
    }
}