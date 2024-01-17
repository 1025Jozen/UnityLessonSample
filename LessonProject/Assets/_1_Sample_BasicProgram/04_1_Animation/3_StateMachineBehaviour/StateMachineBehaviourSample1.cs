using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

//Monobehaviourクラスではなく StateMachineBehaviourクラスを使用するメリット

//・Update→OnStateExitに変更したことで毎フレ監視する必要がなくなって負荷減少
//・モーション開始を検知する必要がなくなったのでコード量削減
//(適切にイベントを検知できるようになった)
//・モーション関連を移したことでMonobehaviourクラスが小さくなる


//【注意点】StateMachineBehaviourはSerializeFieldでシーンにあるオブジェクトを参照できない

namespace StateMachineBehaviourSample
{
    public class StateMachineBehaviourSample1 : StateMachineBehaviour
    {
        [SerializeField]
        KeyCode keycode;

        [SerializeField]
        string parameterName;


        public int i = 20;

        // OnStateEnterは、遷移が始まり、ステートマシンがこの状態の評価を開始するときに呼び出される
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Debug.Log("ステート1に入った");
            Debug.Log(animator.gameObject.transform.position);
            Debug.Log(animator.GetComponent<AnimatorExample>().k);
        }

        // OnStateUpdateは、OnStateEnterコールバックとOnStateExitコールバックの間の各Updateフレームで呼ばれます
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            //Debug.Log("ステートでUpdateループ処理中");
            animator.SetBool(parameterName, Input.GetKey(keycode));

        }

        // OnStateExitは、遷移が終了し、ステートマシンがこの状態の評価を終了したときに呼び出される
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Debug.Log("ステート1から抜けた");
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}


//AnimatorStateInfo
//normalizedTime: アニメーションの正規化された再生時間（0から1の範囲）。この値を使用すると、アニメーションが再生されている特定の位置を把握できます。
//length: アニメーションの再生時間（秒単位）。この値は、アニメーションクリップの長さを示します。
//speed: アニメーションの再生速度。この値を変更することで、アニメーションの再生速度を調整できます。
//loop: アニメーションがループするかどうかを示すブール値。trueの場合、アニメーションはループ再生されます。


//SerializeReference
//SubclassSelector