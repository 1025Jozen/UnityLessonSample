using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SE_BGM
{
    public class SoundSliderSE : MonoBehaviour
    {
        [SerializeField] Slider sliderSE;


        private void Start() {
            sliderSE.value = SoundManager.Instance.GetVolumeSE();
            sliderSE.onValueChanged.AddListener(SetVolumeSE);
        }

        public void SetVolumeSE(float volume) {
            SoundManager.Instance.SetVolumeSE(volume);
        }

        
    }
}


//, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler

//public void OnBeginDrag(PointerEventData eventData) {
//    // ドラッグが始まるときの処理
//    // オブジェクトを持ち上げるなどのアクションを実行
//}

//public void OnDrag(PointerEventData eventData) {
//    // ドラッグ中の処理
//    rectTransform.anchoredPosition += eventData.delta;
//}

//public void OnEndDrag(PointerEventData eventData) {
//    // ドラッグが終了したときの処理
//    // ボタンの上にドロップした場合、必要なアクションを実行
//}

//public void OnPointerUp(PointerEventData eventData) {
//    Debug.Log(sliderSE.value);
//}