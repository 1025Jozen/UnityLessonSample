using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AsyncSample
{
    public class UniTaskExample4 : MonoBehaviour
    {
        async void Start() {            
            int result = await MyAsyncMethod();// 非同期メソッドを呼び出し、返り値を取得する         
            Debug.Log("Async method completed with result: " + result);// 返り値を利用して何かを行う
        }

        private async UniTask<int> MyAsyncMethod() {          
            await UniTask.Delay(1000); // 非同期処理をシミュレート          
            return 42; // 非同期メソッドが整数値を返す場合、その値を返す
        }
    }
    //このサンプルコードでは、MyAsyncMethodという非同期メソッドが整数値を返し、
    //await MyAsyncMethod()でその返り値を待ち、取得しています。
    //取得した返り値を利用して、非同期メソッドが完了した後に何らかの処理を行うことができます。

    //このように、UniTaskを使用することで非同期メソッドの返り値を扱うことができます。
    //必要に応じて非同期メソッドが返す型に合わせて、適切な型を指定してください。
}