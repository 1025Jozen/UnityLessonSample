using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


//基本スクリプト　　このMoveAsync()(目標地点までの移動)を、これをタイムアウトさせてみます。
namespace Timeouts
{
    public class CancelExample2_TimeOut1 : MonoBehaviour
    {
        private void Start()
        {
            _ = MoveStart();
            Debug.Log("先に実行される");
        }

        private async UniTaskVoid MoveStart() {
            transform.position = Vector3.zero;
            //UniTaskはシーンの切り替えや、オブジェクトの破棄では止まらない
            //コルーチンは、StartCoroutineしたGameObjectに紐づくんですが、
            //UniTaskはそういった紐づけはありません。
            //UniTaskの関数を呼ぶ時に引数でthis.GetCancellationTokenOnDestroy()を渡して止める必要がある
            var token = this.GetCancellationTokenOnDestroy();//トークン

            Debug.Log("移動開始！");
            await MoveAsync(new Vector3(8, 0, 0), token);//対象座標まで移動させるメソッド
            Debug.Log("移動終了！");
        }

        /// <summary>オブジェクトが対象座標に到着するまで移動させる</summary>
        private async UniTask MoveAsync(Vector3 targetPosition, CancellationToken cancellationToken) {
            while (true) {               
                var deltaPosition = (targetPosition - transform.position);// 目的の座標　と　自分の座標との差分                
                if (deltaPosition.magnitude < 0.1f) return;               // 0.1m以内に近づいていたら終了
               
                var moveSpeed = 1.0f;                    // 移動速度                
                var direction = deltaPosition.normalized;// 移動方向                
                transform.position += direction * moveSpeed * Time.deltaTime;// 移動させる                
                await UniTask.Yield(cancellationToken);// UniTask.Yieldで1F待つ
            }
        }
    }
}
