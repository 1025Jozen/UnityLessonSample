using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace BrickBreaker_Sample
{
    public class GameClear : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gameClearMessage;

        bool isGameClear = false;// ゲームクリアしたかどうかを管理するフラグを用意

        void Update() {       
            if (transform.childCount == 0) {// 子供がいなくなったら
                gameClearMessage.text = "Game Clear\n<size=50>~press Enter to continue~</size>"; // 追加
                Time.timeScale = 0f; //ゲームを停止する
                                     
                isGameClear = true;  // ゲームクリアのフラグを立てる
            }

            
            if (isGameClear && Input.GetButtonDown("Submit")) {// ゲームクリアしている、かつ、ボタン入力でシーンを再ロード
                Time.timeScale = 1f;           // timeScaleを1に戻しておく                
                SceneManager.LoadScene("BrickBreaker");// シーンのロード
            }

        }
    }
}