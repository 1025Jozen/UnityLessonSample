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





/*    ③awaitするときは、await対象にCancellationTokenが渡せるなら渡す    */

namespace CancelSamples
{
    public class AsyncProcessSample3 : MonoBehaviour
    {
        [SerializeField] KeyCode _destroyObjectKey = KeyCode.D;       
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();// CancellationTokenSourceを用意        
        public CancellationToken Token => _cancellationTokenSource.Token; //※// CancellationToken を CancellationTokenSourceから生成。getできるように

        private void Start()
        {
            //処理の起点　処理を実行
            _ = PrintMessagesAsync(Token);//変数が実際には使用されていないことを明示的に示しただけ
        }

        private void Update() {
            if (Input.GetKeyDown(_destroyObjectKey)) {
                Destroy(this.gameObject);
                Debug.Log("Destroy GameObject");
            }
        }

        /// <summary>
        /// asyncメソッドを定義した場合はCancellationTokenを引数に取る
        /// 作法としては引数の一番最後をCancellationTokenにすることがほとんど
        /// あとメソッド名も ~Async にしておく
        /// </summary>
        public async ValueTask PrintMessagesAsync(CancellationToken token) {
            // これから何か処理をする　　// 受け取ったTokenはすべて下流にも渡す
            await DelayRunAsync(() => Debug.Log("Hello!"), token);//処理を待って、CancellationTokenSource.Token(キャンセル状態を把握できるクラス)を次々に渡していく
            await DelayRunAsync(() => Debug.Log("World!"), token);
            await DelayRunAsync(() => Debug.Log("Bye!")  , token);
        }

        /// <summary> 1秒後にActionを実行する </summary>
        private async ValueTask DelayRunAsync(Action action, CancellationToken token) {            
            await Task.Delay(TimeSpan.FromSeconds(1), token);// 1秒待つ、キャンセル処理を仕込む
            action();
        }

        void OnDestroy() {
            _cancellationTokenSource.Cancel();// クラスを破棄するタイミング・処理をキャンセルしたいタイミングでキャンセル実行            
            _cancellationTokenSource.Dispose();// 破棄
            Debug.Log("キャンセル処理");
        }
    }
}




















