using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //Timelineの制御に必要
using Cinemachine;

using UnityEngine.Timeline;

namespace Timeline
{
    public class TimelineCtrl : MonoBehaviour
    {
        [SerializeField] PlayableDirector playableDirector;
        [SerializeField] CinemachineVirtualCamera virtualCamera1;


        void Start() {
            playableDirector = GetComponent<PlayableDirector>();
            playableDirector.stopped += OnTimelineStopped;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                PlayTimeline();
            }

            if (playableDirector.state == PlayState.Playing) {
                //Debug.Log("Timelineが再生中");
                if (Input.GetKeyDown(KeyCode.W)) {
                    PauseTimeline();
                    Debug.Log("Timelineが一時停止");
                }
            }
        }

        private void OnTimelineStopped(PlayableDirector director) {
            // Timelineの再生が終了したときに呼び出されるメソッド
            Debug.Log("Timelineが再生終了しました");
        }


        //public void OnSignalEmitted(SignalAsset signal) {
        //    // シグナルがトリガーされたときの処理
        //    // ここで位置情報を取得または設定できます
        //    Debug.Log("signal呼び出し");
        //}
        public void OnSignalEmitted1() {
            // シグナルがトリガーされたときの処理
            // ここで位置情報を取得または設定できます
            Debug.Log("signal1呼び出し");
        }
        public void OnSignalEmitted2() {
            // シグナルがトリガーされたときの処理
            // ここで位置情報を取得または設定できます
            Debug.Log("signal2呼び出し");
        }


        //再生する
        void PlayTimeline() {
            playableDirector.Play();
        }

        //一時停止する
        void PauseTimeline() {
            playableDirector.Pause();
        }

        //一時停止を再開する
        void ResumeTimeline() {
            playableDirector.Resume();
        }

        //停止する
        void StopTimeline() {
            playableDirector.Stop();
        }
    }
}