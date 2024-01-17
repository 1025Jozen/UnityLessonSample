using UnityEngine;
using TMPro;

namespace MVP
{
    //View: ユーザーインターフェースを表示および更新する部分
    public class TextView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI textField;

        public void UpdateUI(string text) {
            textField.text = text;
        }
    }
}
