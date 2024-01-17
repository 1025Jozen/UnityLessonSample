using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI_Pop {
    public class ButtonExtension : MonoBehaviour
    {        
        Button button;// 対象のボタン本体のアドレスを保持する変数

        [Header("クリックした時のアニメーション")]
        
        [SerializeField] float _expand_rate; // 拡大率        
        [SerializeField] Ease ease;          // イージング（速度が早いのであんまり意味ないかも）  
        [SerializeField] float expand_time;  // 拡大時間        
        [SerializeField] float contract_time;// 戻り時間        
        Vector3 originalScale;               // サイズをもとに戻すときのオリジナルサイズ


        void Start() {
            button = GetComponentInChildren<Button>();
            originalScale = button.transform.localScale;

            button.onClick.AddListener(OnClick);
        }

        public void OnClick() {
            Sequence sequence = DOTween.Sequence();//Tweenを繋げて1つのアニメーションとして連続実行させることができます
            sequence.Append(button.transform.DOScale(_expand_rate, expand_time)
                .SetRelative()//現在地点から相対値を指定
                .SetEase(ease));//始点と終点をどのように繋ぐかの設定
            sequence.Append(button.transform.DOScale(originalScale, contract_time)
                .SetEase(ease));
            sequence.Play();
        }

        //Append  Sequenceの末尾にTweenを追加します。 前のTweenが終わり次第次のアニメーションが実行
        //AppendInterval / AppendCollback  Appendと同様に末尾に待機時間やコールバックを追加
        //Join    直前のTweenと並行して動作するようにTween追加
        //Insert / InsertCallback
    }
}
