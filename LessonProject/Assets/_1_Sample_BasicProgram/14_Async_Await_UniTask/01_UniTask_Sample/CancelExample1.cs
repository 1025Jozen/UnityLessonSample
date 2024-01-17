using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Antlr3.Runtime;


//C#におけるasync/awaitを使う上で、
//絶対に意識しないといけないものは「キャンセル処理」です。
//正しく処理をキャンセルしないとメモリリークを起こしたり、
//デッドロックやデータ不整合を引き起こす可能性があります。

/*        */

//「結論」
//asyncメソッドはCancellationTokenを引数に取るべき
//await対象が引数にCancellationTokenを要求する場合は省略せずに渡すべき
//OperationCanceledExceptionの取り扱いを意識するべき


/*        */
//「解説」
//「async / awaitにおけるキャンセル」とは2つの意味があります。
//・awaitをキャンセルする
//・await対象の実行中の処理をキャンセルする
//「キャンセル処理」といえばこの2つをまとめて指すことが多いのですが、文脈によっては片方しか意味していないこともあります。

//①「awaitをキャンセルする」
//awaitをキャンセルするとは、「今裏で実行している処理そのものは止めず、待つのをやめる」という意味です。
//「処理が終わるのを待つのを諦める」に近いです。
//たとえば「レストランで注文して料理を作ってもらっているが、時間がかかりすぎているので諦めて店員に何も伝えずに店を出てきた（待つのを止めた）」みたいな。

//②「await対象の実行中の処理をキャンセルする」
//こちらは「裏で走っている処理を止める」という、おそらく「キャンセル処理」という名称からイメージする内容だと思います。
//先程のレストランの例でいうと、「レストランで注文して料理を作ってもらっているが、気が変わったので店員に伝えて作るのを止めてもらった」みたいな。

//*        */

//async / awaitのキャンセル処理では、このどちらを意識すればいいのか
//答:両方意識してください。
//「awaitはキャンセルしたが、処理自体はスレッドプールで走ったままだった」みたいな事故はよく起きます。
//（とくにTask.Runを使っているとき）
//そのため「このキャンセル処理は何を止めればいいのか」をちゃんと把握した上でキャンセルを実装する必要があります。
//ひとまず、これから紹介する内容を守れば「awaitのキャンセル」「await対象の実行処理のキャンセル」の2つは実現できます。

/*        */

//CancellationTokenを使用して非同期タスクを監視し、必要な場合にキャンセルします。
//OnDestroyメソッド内でキャンセルを行い、メモリリークを防ぎます。
namespace AsyncSample
{
    public class CancelExample1 : MonoBehaviour
    {
        [SerializeField] KeyCode _destroyObjectKey = KeyCode.D;
        private CancellationTokenSource cancellationTokenSource;

        private async void Start() {
            Debug.Log("開始");
            cancellationTokenSource = new CancellationTokenSource();//キャンセルしたことを把握するための変数
            try {
                //非同期処理を実行してみる
                await DoSomethingCancelableAsync(cancellationTokenSource.Token);
                Debug.Log("本筋処理終了");
            }

            catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationTokenSource.Token) {

                Debug.Log(ex);
                Debug.Log("Operation canceled");

            }
            //catch (OperationCanceledException) {
            //    Debug.Log("Operation canceled");
            //}
        }

        private void Update()
        {        
            if (Input.GetKeyDown(_destroyObjectKey)) {
                Destroy(this.gameObject);//キャンセル処理したらどうなるか確認用
                Debug.Log("Destroy GameObject");
            }   
        }

        private async UniTask DoSomethingCancelableAsync(CancellationToken cancellationToken) {
            Debug.Log("非同期メソッド開始");
            await UniTask.Delay(5000);                       // 5秒待つ (この時間に処理を止めてみる。）
                                                             // キャンセルトークンを監視し続けて、タスクをキャンセルしてみる
            cancellationToken.ThrowIfCancellationRequested();// タスク内部でキャンセル状態を確認
            Debug.Log("UniTask operation completed");
        }

        private void OnDestroy() {
            // シーン遷移などでGameObjectが破棄される際にキャンセル処理を行う。
            cancellationTokenSource?.Cancel();
            Debug.Log("UniTask operation の　キャンセル処理");
        }
    }
}