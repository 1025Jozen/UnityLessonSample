using UnityEngine;

namespace BrickBreaker_Sample
{
    public class Block : MonoBehaviour
    {
        // 何かとぶつかった時に呼ばれるビルトインメソッド
        void OnCollisionEnter(Collision collision) {            
            Destroy(gameObject);// ゲームオブジェクトを削除するメソッド
        }
    }
}