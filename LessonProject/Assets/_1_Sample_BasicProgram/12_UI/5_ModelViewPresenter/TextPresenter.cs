using UnityEngine;

namespace MVP
{
    // Presenter: ModelとViewの間でデータの受け渡しと制御を行う部分
    public class TextPresenter : MonoBehaviour
    {
        [SerializeField] TextModel model;//Model: データを保持する部分
        [SerializeField] TextView  view; //View : ユーザーインターフェースを表示および更新する部分

        private void Start() {            
            view.UpdateUI(model.Text);// 初期テキストをViewに設定
        }

        public void UpdateText(string newText) {           
            model.Text = newText;     // モデルのデータを更新         
            view.UpdateUI(model.Text);// 更新後のテキストをViewに反映
        }
    }
}