using UnityEngine;
using TMPro;

//GameObjectにアタッチ
namespace VContainer3
{
    //View: ユーザーインターフェースを表示および更新する部分
    public class GameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sampleText;
        public void SetSampleText(string text) {
            sampleText.text = text;
        }
    }
}