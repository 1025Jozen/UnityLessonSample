using UnityEngine;
using Cinemachine;


namespace TargetLock
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;

        private void Update() {
            // シフトキーが押されているかを確認
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                virtualCamera.enabled = true;

            } else {
                virtualCamera.enabled = false;
            }
        }
    }
}