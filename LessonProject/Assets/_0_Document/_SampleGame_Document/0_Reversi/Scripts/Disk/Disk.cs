using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

namespace Othello
{
    public class Disk : MonoBehaviour
    {
        [SerializeField] private GameObject _black;
        [SerializeField] private GameObject _white;
        [SerializeField] private GameObject _dot;


        public enum DiskColor
        {
            Black,
            White
        }
        public enum DiskState
        {
            None,
            Appearing,
            Reversing,
            Fit
        }
        public DiskColor _diskColor { get; private set; } = DiskColor.Black;//石の色の状態
        public DiskState _diskState { get; private set; } = DiskState.None; //石の状態








        Quaternion Rotation
        {
            get
            {
                switch (_diskColor) {
                    case DiskColor.Black:
                        return Quaternion.Euler(0, 0, 0);
                    case DiskColor.White:
                    default:
                        return Quaternion.Euler(0, 0, 180);
                }
            }
        }

        public void EnableDot() {
            this._black.SetActive(false);
            this._white.SetActive(false);
            this._dot.SetActive(true);
            gameObject.SetActive(true);
        }

        public void DisableDot() {
            this._black.SetActive(true);
            this._white.SetActive(true);
            this._dot.SetActive(false);
            //gameObject.SetActive(false);
        }


        /// <summary>
        /// 石の表示
        /// </summary>
        /// <param name = "activeValue" > trueで石を表示 </ param >
        /// < param name="diskColor">Blackとして表示するか、Whiteとして表示するか</param>
        public void SetActive(bool activeValue, DiskColor diskColor) {

            if (activeValue)//セットするなら石の状態を変える　　
            {
                this._diskColor = diskColor;          //該当の色に変える　
                this._diskState = DiskState.Appearing;//該当の状態に変える
                DisableDot();

                _stateChangedAt = DateTime.UtcNow;

            } else  //セットしないなら
              {
                this._diskState = DiskState.None;
            }
            this.gameObject.SetActive(activeValue);//セット　置く　表示する
        }


        //ひっくり返ってもらう
        public void Reverse() {
            if (_diskState == DiskState.None)//石の状態　が　表示されていない状態ならそのままreturn
                return;

            _diskState = DiskState.Reversing;//ひっくり返し状態に変更

            switch (_diskColor) {//色の状態を反対に変える
                case DiskColor.Black:
                    _diskColor = DiskColor.White;
                    break;
                case DiskColor.White:
                    _diskColor = DiskColor.Black;
                    break;
            }


            _stateChangedAt = DateTime.UtcNow;//時間

        }
        private static readonly float AppearSeconds = 0.5f;
        private static readonly float ReverseSeconds = 0.5f;

        private DateTime _stateChangedAt = DateTime.MinValue;
        private float ElapsedSecondsSinceStateChanged { get { return (float)(DateTime.UtcNow - _stateChangedAt).TotalSeconds; } }


        private void Update() {
            switch (_diskState) {
                case DiskState.Appearing:
                    //{
                    //    //黒もしくは白に回転
                    //    transform.localRotation = Rotation;
                    //    _diskState = DiskState.Fit;
                    //    break;
                    //}


                    transform.localRotation = Rotation;
                    //position animation
                    {

                        var startPos = transform.localPosition;
                        var endPos = startPos;
                        startPos.y = 1;
                        endPos.y = 0;
                        var t = Mathf.Clamp01(1 - ElapsedSecondsSinceStateChanged / AppearSeconds);
                        t = 1 - t * t * t * t;
                        transform.localPosition = Vector3.Lerp(startPos, endPos, t);
                        if (AppearSeconds < ElapsedSecondsSinceStateChanged) {
                            transform.localPosition = endPos;
                            _diskState = DiskState.Fit;
                        }
                    }
                    //_diskState = DiskState.Fit;
                    break;
                case DiskState.Reversing:
                    //{
                    //    transform.localRotation = Rotation;
                    //    _diskState = DiskState.Fit;
                    //    break;
                    //}
                    {
                        //rotation animation
                        var startRot = Quaternion.identity;
                        var endRot = Rotation;
                        switch (_diskColor) {
                            case DiskColor.Black:
                                startRot = Quaternion.Euler(0, 0, 180);
                                break;
                            case DiskColor.White:
                                startRot = Quaternion.Euler(0, 0, 0);
                                break;
                        }
                        var t = Mathf.Clamp01(1 - ElapsedSecondsSinceStateChanged / ReverseSeconds);
                        t = 1 - t * t * t * t;
                        transform.localRotation = Quaternion.Lerp(startRot, endRot, t);

                        //position animation
                        var maxY = 1f;
                        t = Mathf.Clamp01(ElapsedSecondsSinceStateChanged / ReverseSeconds);
                        var pos = transform.localPosition;
                        pos.y = maxY * Mathf.Sin(t * Mathf.PI);
                        transform.localPosition = pos;

                        if (ReverseSeconds < ElapsedSecondsSinceStateChanged) {
                            pos.y = 0;
                            transform.localPosition = pos;
                            transform.localRotation = endRot;
                            _diskState = DiskState.Fit;
                        }
                    }
                    break;
                case DiskState.Fit:
                case DiskState.None:
                    break;
            }
        }
    }


    //public void SetActice(bool activeValue)
    //{
    //    this.gameObject.SetActive(activeValue);
    //}
}