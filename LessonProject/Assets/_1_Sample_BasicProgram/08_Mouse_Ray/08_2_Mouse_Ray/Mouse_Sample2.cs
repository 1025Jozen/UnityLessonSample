using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mouse_Sample1 {
    public class Mouse_Sample2 : MonoBehaviour
    {
        //[SerializeField] private GameObject m_object = null;

        private void Update() {
            Vector3 touchScreenPosition = Input.mousePosition;//マウスの座標
            touchScreenPosition.x = Mathf.Clamp(touchScreenPosition.x, 0.0f, Screen.width);
            touchScreenPosition.y = Mathf.Clamp(touchScreenPosition.y, 0.0f, Screen.height);

            Camera gameCamera           = Camera.main;
            Ray touchPointToRay = gameCamera.ScreenPointToRay(touchScreenPosition);//マウス位置からレイを出す
            Debug.DrawRay(touchPointToRay.origin, touchPointToRay.direction * 1000.0f);
            //m_object.transform.position = touchWorldPosition;
        }
    }
}

