using System;
using System.Threading;
using UnityEngine;

//async / awaitのキャンセル方法
//ここからが本題。async/awaitにキャンセル処理をつける場合はどうしたらいいのか。
//結論からいうと、async/awaitにおけるキャンセル処理では、次の一連の流れをすべて実装する必要があります。

//①CancellationTokenを適切なタイミングで生成し、キャンセルしたいタイミングでキャンセル状態にする
//②asyncメソッドを定義するときはCancellationTokenを引数にとる
//③awaitするときは、await対象にCancellationTokenが渡せるなら渡す
//④await対象にCancellationTokenが渡せないのであれば、CancellationToken.ThrowIfCancellationRequested()を適宜呼び出す
//⑤async/awaitとtry-catchを併用する場合はOperationCancelledExceptionの扱いを考える




/*    ①CancellationTokenを適切なタイミングで生成し、キャンセルしたいタイミングでキャンセル状態にする    */
//CancellationTokenとは、async/awaitにおいて「処理のキャンセルを伝えるためのオブジェクト」
//このCancellationTokenを適切なタイミングで生成し、
//処理を中止したいタイミングでキャンセル状態に変更することでasync/awaitをキャンセルさせることができます



//具体的には、CancellationTokenSourceを使ってCancellationTokenを生成します。
//この親となったCancellationTokenSourceのCancel()を呼び出すことで、
//ここから発行されたCancellationTokenがキャンセル状態になります。



namespace CancelSamples
{
    /// <summary> たとえば、このクラスのインスタンスの寿命に紐づけたCancellationTokenが欲しい場合 </summary>
    public class AsyncProcessSample1 : MonoBehaviour
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
    }
}




















