using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;
using System;

public class CancelExample3_UIButton : MonoBehaviour
{
    private CancellationTokenSource cancellationTokenSource;
    private bool isTaskRunning;

    [SerializeField] private Button startButton;
    [SerializeField] private Button cancelButton;

    private void Start() {
        Debug.Log("Startメソッド開始");

        cancellationTokenSource = new CancellationTokenSource();
        isTaskRunning = false;//初期設定

        startButton.onClick.AddListener(StartAsyncOperation);  // 開始ボタンにメソッドを設定        
        cancelButton.onClick.AddListener(CancelAsyncOperation);// キャンセルボタンにメソッドを設定
    }

    //スタートボタンを押した時
    private async void StartAsyncOperation() {
        if (isTaskRunning) {//bool値でタスクが走っているかどうかを見る
            Debug.Log("Task is already running.");
            return;
        }

        Debug.Log("Starting async operation");

        try {
            isTaskRunning = true; //           
            await DoSomethingCancelableAsync(cancellationTokenSource.Token);// キャンセル可能な非同期メソッドを呼び出す
            Debug.Log("Async method completed");//非同期メソッドの処理終了
        }
        catch (OperationCanceledException) {//トークンのキャンセルで呼び出される
            Debug.Log("Operation canceled");
        }
        finally {
            isTaskRunning = false;//最終的にはfalseに戻しとく
        }
    }

    private async UniTask DoSomethingCancelableAsync(CancellationToken cancellationToken) {
        Debug.Log("Doing something asynchronously");

        try {
            // キャンセルトークンを監視してタスクをキャンセル可能にする
            await UniTask.Delay(5000); // 5秒待つ

            // タスク内部でキャンセル状態を確認
            //ここまでに実行された関数はキャンセルできないが、
            //ここまでにキャンセルが呼ばれたかどうかの確認ができる。
            //ここより下に書くと確認ができないので、エラーが出そうなメソッドなどあるなら何回か書く必要がある
            cancellationToken.ThrowIfCancellationRequested();

            Debug.Log("Async operation completed");
        }
        catch (OperationCanceledException) {//タスクのキャンセルが確認できたら
            Debug.Log("Async operation canceled");
            throw;
            // OperationCanceledExceptionを再スローしてキャッチできるようにする
            //throwすることで元のキャンセル例外が保持されます。
            //これにより、キャンセルが発生した元のコンテキストやスタックトレースが維持され、
            //デバッグやログのトラッキングが容易になります。
            //簡単に言えば、throw; ステートメントは、キャンセルが要求された場合に、
            //その要求を正しく処理し、例外がキャッチされたことを示すために使用されます。
        }
    }

    //キャンセルボタンを押した時
    private void CancelAsyncOperation() {
        if (isTaskRunning) {
            // タスクが実行中であればキャンセルを実行
            cancellationTokenSource.Cancel();
        }
    }
}
