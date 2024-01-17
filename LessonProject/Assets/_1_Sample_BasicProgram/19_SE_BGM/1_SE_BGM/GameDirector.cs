using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SE_BGM
{
    public class GameDirector : MonoBehaviour
    {
        const int COUNTDOWN_SEC = 3;
        [SerializeField] TextMeshProUGUI textCountdown;
        [SerializeField] GameObject bgm_se_Canvas;

        private void Start() {
            SoundManager.Instance.PlayBGM();
            bgm_se_Canvas.SetActive(false);
            StartCoroutine(countdown());
        }

        IEnumerator countdown() {
            for (int i = COUNTDOWN_SEC; i > 0; i--) {                           
                yield return new WaitForSeconds(1);// 1秒まつ
                textCountdown.text = i + "";
                SoundManager.Instance.PlaySE(0);
                Debug.Log(i);
            }
            yield return new WaitForSeconds(1);
            SoundManager.Instance.PlaySE(1);
            textCountdown.text = "START !!";                    
            yield return new WaitForSeconds(0.5f);
            textCountdown.text = "";
            bgm_se_Canvas.SetActive(true);

        }

        private void Update()
        {
            if (Input.anyKey) {
                SoundManager.Instance.PlaySE(2);
            }
        }
    }
}

