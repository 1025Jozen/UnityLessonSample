using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool_Sample : MonoBehaviour
{
    void Start()
    {
        // インスタンス化の例
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc     : ()     => GameObject.CreatePrimitive(PrimitiveType.Cube),// プールが空のときに新しいインスタンスを生成する処理
            actionOnGet    : target => target.SetActive(true),                        // プールから取り出されたときの処理 
            actionOnRelease: target => target.SetActive(false),                       // プールに戻したときの処理
            actionOnDestroy: target => Destroy(target),                               // プールがmaxSizeを超えたときの処理
            collectionCheck: true,                                                    // 同一インスタンスが登録されていないかチェックするかどうか
            defaultCapacity: 10,                                                      // デフォルトの容量
            maxSize: 100);
    }

    void Update()
    {
        
    }
}
