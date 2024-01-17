using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mouse_Sample1 {
    public class Mouse_Sample3 : MonoBehaviour
    {
        [SerializeField] private GameObject m_object = null;

        private void Update() {
            Vector2 touchScreenPosition = Input.mousePosition;//マウス座標
            touchScreenPosition.x = Mathf.Clamp(touchScreenPosition.x, 0.0f, Screen.width);
            touchScreenPosition.y = Mathf.Clamp(touchScreenPosition.y, 0.0f, Screen.height);

            Camera gameCamera = Camera.main;
            Ray touchPointToRay = gameCamera.ScreenPointToRay(touchScreenPosition);//カメラからレイを飛ばす

            RaycastHit hitInfo = new RaycastHit();//レイに関する情報を入れる
            if (Physics.Raycast(touchPointToRay, out hitInfo)) {//もしレイが当たり判定を検知したら
                m_object.transform.position = hitInfo.point;//Objectの位置を　検知した位置に持ってくる
            }
            Debug.DrawRay(touchPointToRay.origin, touchPointToRay.direction * 1000.0f);
        }
    }
}

