using UnityEngine;
using Cysharp.Threading.Tasks;


//UniTaskというライブラリをとってきて、UniTaskを使用して非同期メソッドを定義します。
//UniTask.Delayを使用して待機します。
namespace AsyncSample
{
    public class AsyncSample2 : MonoBehaviour
    {
        private async void Start() {
            Debug.Log("処理開始①");         
            await DoSomethingAsync();// 非同期メソッドを呼び出して、待つ。
            Debug.Log("本筋処理終了④");
        }

        private async UniTask DoSomethingAsync() {
            Debug.Log("非同期処理が動②");
            await UniTask.Delay(1000); // 1秒待つ
            Debug.Log("UniTask operation completed,非同期で動かした処理の終了③");
        }
    }
}