using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SE_BGM {
    public class SoundManager : MonoBehaviour
    {        
        public static SoundManager Instance;// どこからでも参照できるようにしておく

        AudioSource seAudioSource;
        AudioSource bgmAudioSource;

        [SerializeField] AudioClip bgmClip;
        [SerializeField] AudioClip[] seClips;

        private void Awake() {
            // テストのときに面倒だが、各シーンで参照できるように
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(this);
            } else {
                Destroy(gameObject);
            }

            // AudioSourceのコンポーネントが無かったら作っておく（インスペクタから入れるなら不要）
            if (seAudioSource == null) {
                seAudioSource = gameObject.AddComponent<AudioSource>();
            }
            if (bgmAudioSource == null) {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
                bgmAudioSource.volume = 0.1f;
                bgmAudioSource.loop = true;
            }
        }

        /// <summary>BGMを再生します</summary>
        public void PlayBGM() {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.Play();
        }

        /// <summary>SEを鳴らします<br></br>番号はインスペクタに入れた順です</summary>
        /// <param name="num"></param>
        public void PlaySE(int num) {
            if (num > seClips.Length) {
                return;
            }
            seAudioSource.clip = seClips[num];//配列内の何番目をPlayするか
            seAudioSource.Play();
        }

        /// <summary>SE音量を設定します</summary>
        /// <param name="volume"></param>
        public void SetVolumeSE(float volume) {
            seAudioSource.volume = volume;
        }

        /// <summary>BGM音量を設定します</summary>
        /// <param name="volume"></param>
        public void SetVolumeBGM(float volume) {
            bgmAudioSource.volume = volume;
        }


        public float GetVolumeSE() {
            return seAudioSource.volume;
        }

        public float GetVolumeBGM() {
            return bgmAudioSource.volume;
        }

        public void StopBGM() {
            bgmAudioSource.Stop();
        }
    }
}
