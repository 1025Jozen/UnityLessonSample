using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Countdown {
    public class UI_Countdown : MonoBehaviour
    {
        const int COUNTDOWN_SEC = 5;
        [SerializeField] TextMeshProUGUI textCountdown;

        private void Start() {
            StartCoroutine(countdown());
        }

        IEnumerator countdown() {
            for (int i = COUNTDOWN_SEC; i > 0; i--) {
                textCountdown.text = i + "";//テキスト変更

                //カウントダウン音を鳴らすスクリプトを入れたり
               
                yield return new WaitForSeconds(1);// 1秒まつ
            }
            //スタート音を鳴らすスクリプトを入れたり


            textCountdown.text = "START !!";//テキスト変更

            //ゲーム開始のフラグをONにするスクリプトを入れたり


        }
    }
}
