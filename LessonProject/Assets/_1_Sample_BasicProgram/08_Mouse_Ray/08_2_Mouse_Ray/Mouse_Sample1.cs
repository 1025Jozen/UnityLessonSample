using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mouse_Sample1 {
    public class Mouse_Sample1 : MonoBehaviour
    {
        [SerializeField] private GameObject m_object = null;

        private void Update() {
            Vector3 touchScreenPosition = Input.mousePosition;//マウスの座標
            touchScreenPosition.x = Mathf.Clamp(touchScreenPosition.x, 0.0f, Screen.width);//ゲーム画面内のみという制限をかける
            touchScreenPosition.y = Mathf.Clamp(touchScreenPosition.y, 0.0f, Screen.height);
            touchScreenPosition.z       = 10.0f;// 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.

            Camera gameCamera           = Camera.main;
            Vector3 touchWorldPosition  = gameCamera.ScreenToWorldPoint(touchScreenPosition);//３Ｄ空間のワールド座標に変換

            m_object.transform.position = touchWorldPosition;
        }
    }
}

