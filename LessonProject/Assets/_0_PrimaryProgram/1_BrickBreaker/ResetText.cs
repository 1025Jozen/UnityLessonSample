using UnityEngine;
using TMPro;

namespace BrickBreaker_Sample
{
    public class ResetText : MonoBehaviour
    {
        void Start() {
            // アクセスは1回きりなので、フィールド変数を用意していない
            TextMeshProUGUI _myText = GetComponent<TextMeshProUGUI>();
            _myText.text = ""; // textに空の文字列を設定する
        }

    }
}