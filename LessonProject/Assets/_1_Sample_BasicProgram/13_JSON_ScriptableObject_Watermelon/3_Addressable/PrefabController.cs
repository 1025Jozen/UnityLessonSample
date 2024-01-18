//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

//public class PrefabController : MonoBehaviour
//{
//    private AsyncOperationHandle<GameObject> m_CubeHandle;

//    private void Start() {
//        m_CubeHandle = Addressables.InstantiateAsync("Cube", new Vector3(0, 0, 0), Quaternion.identity);
//    }

//    private void OnDestroy() {
//        if (m_CubeHandle.IsValid()) Addressables.Release(m_CubeHandle);
//    }
//}