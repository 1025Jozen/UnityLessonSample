using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Timeouts
{
    public class CancelExample2_TimeOut2 : MonoBehaviour
    {
        private void Start() {
            _ = MoveStart();
            Debug.Log("先に実行される");
        }

        private async UniTaskVoid MoveStart() {
            transform.position = Vector3.zero;
            var timeoutController = new TimeoutController();// TimeoutControllerを生成
            Debug.Log("移動開始！");

            try {
                // TimeoutControllerから指定時間後にキャンセルされるCancellationTokenを生成
                var timeoutToken = timeoutController.Timeout(TimeSpan.FromSeconds(1));//タイムアウト設定

                // このGameObjectが破棄されたらキャンセルされるCancellationTokenを生成
                var destroyToken = this.GetCancellationTokenOnDestroy();

                // タイムアウトとDestroyのどちらもでキャンセルするようにTokenを生成
                var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(timeoutToken, destroyToken).Token;

                // 1秒でタイムアウトさせてみる
                await MoveAsync(new Vector3(8, 0, 0), linkedToken);

                // 使い終わったらReset()してあげる必要あり
                timeoutController.Reset();

                Debug.Log("移動終了");
            }
            catch (Exception ex) {
                Debug.LogException(ex);//警告にする必要はない
                if (timeoutController.IsTimeout()) {
                    Debug.LogError("Timeoutによるキャンセルです");
                }
            }
        }

        /// <summary>オブジェクトが対象座標に到着するまで移動させる</summary>
        private async UniTask MoveAsync(Vector3 targetPosition, CancellationToken ct) {
            while (true) {                
                var deltaPosition = (targetPosition - transform.position);// 目的座標までの差分        
                if (deltaPosition.magnitude < 0.1f) return;// 0.1m以内に近づいていたら終了               
                var moveSpeed = 1.0f;                      // 移動速度              
                var direction = deltaPosition.normalized;  // 移動方向              
                transform.position += direction * moveSpeed * Time.deltaTime;// 移動させる              
                await UniTask.Yield(ct);// 1F待つ
            }
        }
    }
}
