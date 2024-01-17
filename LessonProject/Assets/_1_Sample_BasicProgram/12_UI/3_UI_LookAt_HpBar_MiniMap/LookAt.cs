using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LookAt : MonoBehaviour
    {
        private enum Mode {
            LookAt,        //カメラの方を見る
            LookAtInverted,//カメラと同じ方向を見る
            CameraForward, //カメラ基準で、カメラが向いてる方向
            CameraForwardInverted//カメラ基準で、カメラが向いてる逆方向
                //world座標基準　 UIをつけたobject基準など色々考えられる
        }

        [SerializeField] Mode mode;

        private void LateUpdate()
        {
            switch (mode) {
                case Mode.LookAt:
                    transform.LookAt(Camera.main.transform);//UIがカメラの方向を向く
                    break;
                case Mode.LookAtInverted:
                    Vector3 dirFromCamera = transform.position - Camera.main.transform.position;//カメラから見た方向
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case Mode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                case Mode.CameraForwardInverted:
                    transform.forward = - Camera.main.transform.forward;
                    break;
            }

            transform.LookAt(Camera.main.transform);
        }

    }
}