using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OnePoint_Effect
{
    public class TrailEffect : MonoBehaviour
    {        
        [SerializeField] Camera tap_camera;  // 座標変換するためのカメラ

        [SerializeField] GameObject TrailEffectsPrefab;// トレイルのエフェクト
        GameObject _trail;// 実体化したトレイル        
        bool isTrailing;// トレイル中かどうか
        
        public Vector3 cursor_point { private set; get; }// 現在のポインター位置（マウス、タップでドラッグ）
        
        [SerializeField] float trail_z = -2;  // カメラ前面に出したい


        private InputSys _inputs;

        private void OnEnable() {
            _inputs = new InputSys();
            _inputs.UI.Click.performed   += OnClick;
            _inputs.UI.Click.canceled    += OnClick;
            _inputs.UI.Pointer.performed += OnPointer;
            _inputs.Enable();
        }
        private void OnDisable() {
            _inputs.UI.Pointer.performed -= OnPointer;
            _inputs.UI.Click.performed   -= OnClick;
            _inputs.UI.Click.canceled    -= OnClick;
            _inputs.Disable();
        }





        private void Start() {
            _trail = Instantiate(TrailEffectsPrefab);
            _trail.transform.SetParent(gameObject.transform);
            Trail(false);
        }


        public void OnPointer(InputAction.CallbackContext context) {
            if (context.performed) {
                Vector2 tmp = context.ReadValue<Vector2>();// 位置を取得                
                cursor_point = tap_camera.ScreenToWorldPoint(new Vector3(tmp.x, tmp.y));// 座標変換                
                cursor_point = new Vector3(cursor_point.x, cursor_point.y, trail_z);// 奥行Zを設定しておく
            }
        }

        public void OnClick(InputAction.CallbackContext context) {
            if (context.performed) {
                Debug.Log("ボタン押した");
                Trail(true);

            } else if (context.canceled) {
                Debug.Log("ボタン離した");
                Trail(false);
            }
        }

        private void FixedUpdate() {
            if (isTrailing) {
                _trail.transform.position = cursor_point;//trailの位置をマウスの位置へ
            }
        }

        public void Trail(bool isOn) {
            isTrailing = isOn;
            _trail.SetActive(isOn);
            _trail.transform.position = cursor_point;
        }
    }

}

