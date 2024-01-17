using UnityEngine;
using System;


namespace UnityEvent_Sample
{
    //action から View 関数を除外するときは「onCollisionEnter.action -= View;」のように書きます。
    //action に関数が渡されていない状態で Invoke を呼ぶとエラーになります。基本的にNullチェックしましょう。Null条件演算子を使えば簡潔にNullチェックできます。例：action?.Invoke();
    //action の宣言の前についている event を取ると、他のクラスからも Invoke を呼べるようになります。誰が呼んだか分からなくなるので注意して下さい。


    public class OnCollisionEnterEvent : MonoBehaviour
    {
        [SerializeField] public event Action<Collision> collisionAction;

        private void OnCollisionEnter(Collision collision) {
            collisionAction.Invoke(collision);//collisionActionに登録されている関数実行
        }
    }
}
