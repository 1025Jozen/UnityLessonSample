//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.UI;

//public class EnemyController : MonoBehaviour
//{

//    [SerializeField] private Text m_Text;
//    private AsyncOperationHandle<MyData> m_EnemyHandle;

//    private void Start() {
//        Addressables.LoadAssetAsync<MyData>("Enemy").Completed += handle => {
//            m_EnemyHandle = handle;
//            if (handle.Result == null) {
//                Debug.Log("Load Error");
//                return;
//            }

//            var enemy = handle.Result;
//            m_Text.text = $"{enemy.Name} : {enemy.Hp}";
//        };
//    }

//    private void OnDestroy() {
//        if (m_EnemyHandle.IsValid()) Addressables.Release(m_EnemyHandle);
//    }
//}