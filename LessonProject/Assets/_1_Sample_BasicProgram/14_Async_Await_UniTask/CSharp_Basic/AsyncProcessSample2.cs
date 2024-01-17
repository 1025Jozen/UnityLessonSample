using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

//async / awaitのキャンセル方法
//ここからが本題。async/awaitにキャンセル処理をつける場合はどうしたらいいのか。
//結論からいうと、async/awaitにおけるキャンセル処理では、次の一連の流れをすべて実装する必要があります。

//①CancellationTokenを適切なタイミングで生成し、キャンセルしたいタイミングでキャンセル状態にする
//②asyncメソッドを定義するときはCancellationTokenを引数にとる
//③awaitするときは、await対象にCancellationTokenが渡せるなら渡す
//④await対象にCancellationTokenが渡せないのであれば、CancellationToken.ThrowIfCancellationRequested()を適宜呼び出す
//⑤async/awaitとtry-catchを併用する場合はOperationCancelledExceptionの扱いを考える





/*    ②asyncメソッドを定義するときはCancellationTokenを引数にとる    */
//CancellationTokenが用意できているなら、これをasyncメソッドに渡す必要があります。
//そのためにもasyncメソッドはCancellationTokenを引数にとるようにしましょう。

namespace CancelSamples
{
    public class AsyncProcessSample2 : MonoBehaviour
    {
        [SerializeField] KeyCode _destroyObjectKey = KeyCode.D;       
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();// CancellationTokenSourceを用意

        //// ※　// CancellationToken を CancellationTokenSourceから生成。getできるように
        //public CancellationToken Token => _cancellationTokenSource.Token;

        private void Update() {
            if (Input.GetKeyDown(_destroyObjectKey)) {
                Destroy(this.gameObject);
                Debug.Log("Destroy GameObject");
            }
        }

        void OnDestroy() {
            _cancellationTokenSource.Cancel();// クラスを破棄するタイミング・処理をキャンセルしたいタイミングでキャンセル実行            
            _cancellationTokenSource.Dispose();// 破棄
            Debug.Log("キャンセル処理");
        }



        /// <summary>
        /// asyncメソッドを定義した場合はCancellationTokenを引数に取る
        /// 作法としては引数の一番最後をCancellationTokenにすることがほとんど
        /// あとメソッド名も ~Async にしておく
        /// </summary>
        //public async ValueTask DelayRunAsync(Action action, CancellationToken token) {
        //    // これから何か処理をする
        //    //await 時間のかかる処理・待つ処理;
        //}
    }
}




















