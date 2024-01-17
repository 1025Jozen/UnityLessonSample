using UnityEngine;
using UnityEngine.Events;

namespace UnityEvent_Sample {
    public class UnityEvent_Sample : MonoBehaviour
    {
        public UnityEvent<Collision> Event;

        private void OnCollisionEnter(Collision collision) {
            // Event が受け取った各関数の引数に collisionを代入して、各関数を実行
            Event.Invoke(collision);
        }

        //UnityEvent に追加する関数の引数が1つだけの場合に限り、インスペクタ上で自由な値を代入できます（関数の引数が一致している必要もありません）。
        //UnityEvent に追加する関数の引数が2つ以上の場合は、スクリプトから関数を追加するか、引数が全て一致する関数を指定する必要があります。
        //プルダウンメニューに関数が出てこない場合は、関数が public になっていない可能性があるか、戻り値が void になっていない可能性があります

    }
}

