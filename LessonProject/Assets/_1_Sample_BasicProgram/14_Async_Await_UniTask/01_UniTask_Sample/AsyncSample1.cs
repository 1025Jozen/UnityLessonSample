using System;
using System.Threading.Tasks;
using UnityEngine;


//async修飾子を使用して非同期メソッドを定義します。
//awaitキーワードを使用して非同期操作を待ちます。
namespace AsyncSample
{
    public class AsyncSample1 : MonoBehaviour
    {
        private async void Start() {
            Debug.Log("処理開始①");            
            await DoSomethingAsync();// 非同期メソッドの処理を待つ
            Debug.Log("本筋処理終了④");
        }

        private async Task DoSomethingAsync() {
            Debug.Log("非同期に動かす(asynchronousの略。)②");
            await Task.Delay(1000); // 1秒待つ。1000m秒
            Debug.Log("非同期で動かす処理の終了③");
        }
    }
}