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





/*    ④await対象にCancellationTokenが渡せないのであれば、CancellationToken.ThrowIfCancellationRequested()を適宜呼び出す    */
//CancellationToken.ThrowIfCancellationRequested()メソッドは、
//「CancellationTokenがキャンセル状態になっていたときに、
//  OperationCanceledExceptionを発行する」 というメソッドです。

//あとで後述しますが、async/awaitでは（正確にいうとTaskたちは）
//OperationCanceledExceptionを特殊な例外として扱っています。

//もしawait実行時にCancellationTokenが相手に渡せない場合は、
//キャンセル時にこのOperationCanceledExceptionを発行してあげる必要があります。
//これらを自動でやってくれるのがCancellationToken.ThrowIfCancellationRequested()です。

namespace CancelSamples
{
    public class AsyncProcessSample4 : MonoBehaviour
    {
        [SerializeField] KeyCode _destroyObjectKey = KeyCode.D;       
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();// CancellationTokenSourceを用意        
        public CancellationToken Token => _cancellationTokenSource.Token; //※// CancellationToken を CancellationTokenSourceから生成。getできるように

        private void Start()
        {
            _ = PrintMessagesAsync(Token);//変数が実際には使用されていないことを明示的に示しただけ
        }

        private void Update() {
            if (Input.GetKeyDown(_destroyObjectKey)) {
                Destroy(this.gameObject);
                Debug.Log("Destroy GameObject");
            }
        }

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



        // 何か別のライブラリの非同期メソッドを実行したいが、そのライブラリのお行儀が悪く
        // CancellationTokenを渡すことができない場合など
        //private async ValueTask UseOtherFrameworkAsync(CancellationToken token) {
        //    // NankanoAsync()自体は走り出したらキャンセルできないので諦めるとして、
        //    //await なんかのライブラリ.NankanoAsync();

        //    // キャンセル状態になっていたらこの時点で処理を止める
        //    // （例外が発行されてここで中断される）
        //    token.ThrowIfCancellationRequested();

        //    // SugoiAsync()も走り出したら後から止めることはできない（諦める）
        //    //await なんかのライブラリ.SugoiAsync();
        //}
    }
}




















