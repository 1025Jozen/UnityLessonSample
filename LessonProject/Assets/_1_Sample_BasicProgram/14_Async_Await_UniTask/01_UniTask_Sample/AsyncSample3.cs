using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AsyncSample
{
    public class UniTaskExample3 : MonoBehaviour
    {
        async void Start() {
            Debug.Log("Start①");

            // 並列で3つの非同期タスクを実行。ここでは処理終了は待たない
            var task1 = DoTaskAsync(1);
            var task2 = DoTaskAsync(2);
            var task3 = DoTaskAsync(3);
           
            await UniTask.WhenAll(task1, task2, task3);// すべてのタスクが完了するのを待つ

            Debug.Log("All tasks completed⑧");
        }

        private async UniTask DoTaskAsync(int id) {
            Debug.Log($"Start UniTask operation: {id}");
            await UniTask.Delay(id * 1000); // 時間のかかる非同期処理をシミュレート(異なる時間を待つ)
            Debug.Log($"Task {id} completed");
        }
    }
}