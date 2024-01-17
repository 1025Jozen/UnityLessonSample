using System.Collections;
using System.Collections.Generic;
using SE_BGM;
using UnityEngine;
using UnityEngine.UI;


namespace SE_BGM
{
    public class SoundSliderBGM : MonoBehaviour
    {
        [SerializeField] Slider sliderBGM;

        private void Start() {
            sliderBGM.value = SoundManager.Instance.GetVolumeBGM();
        }
        public void SetVolumeBGM(float volume) {
            SoundManager.Instance.SetVolumeBGM(volume);
        }
    }
}