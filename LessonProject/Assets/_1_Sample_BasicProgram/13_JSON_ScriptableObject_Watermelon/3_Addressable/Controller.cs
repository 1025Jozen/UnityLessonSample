//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.UI;

//public class Controller : MonoBehaviour
//{
//    [SerializeField] private Image m_Image;

//    private AsyncOperationHandle<Sprite> m_SpriteHandle;

//    private void Start() {
//        Addressables.LoadAssetAsync<Sprite>("apple").Completed += handle => {
//            m_SpriteHandle = handle;
//            if (handle.Result == null) {
//                Debug.Log("Load Error");
//                return;
//            }
//            m_Image.sprite = handle.Result;
//        };
//    }

//    private void OnDestroy() {
//        if (m_SpriteHandle.IsValid()) Addressables.Release(m_SpriteHandle);
//    }
//}