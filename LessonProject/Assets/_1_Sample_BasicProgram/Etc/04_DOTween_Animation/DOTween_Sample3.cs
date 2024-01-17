using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DOTween_Sample {
    public class DOTween_Sample3 : MonoBehaviour
    {

        IEnumerator Start() {

            //transform.DOLocalMove(new Vector3(10f, 0, 0), 10f);// 移動トゥイーン

            //// 回転トゥイーン
            //transform.DORotateQuaternion(Quaternion.AngleAxis(90f, Vector3.up), 1f)
            //    .SetLoops(-1, LoopType.Incremental)
            //    .SetEase(Ease.Linear);



            transform.DOLocalMove(new Vector3(10f, 0, 0), 10f);
            transform.DOLocalRotate(new Vector3(180f, 45f, -100f), 10f);
            transform.DOScale(new Vector3(4f, 0.24f, 2f), 10f);


            yield return new WaitForSeconds(0.5f);
            transform.DOPause();// トゥイーンの一時停止 Debug.Logで出るint数はトゥイーン数
            yield return new WaitForSeconds(0.5f);            
            transform.DOPlay();// トゥイーンの再開


            //transform.DOLookAt(lookAtTarget.localPosition, 1f);
        }
    }


    //var sequence = DOTween.Sequence(); //Sequence生成
    //Tweenを繋げて1つのアニメーションとして連続実行させることができ、
    //複雑なアニメーションを作ることができます。

    //Append   前のTweenが終わる時間までDelay
    //AppendInterval / AppendCollback   Appendと同様に末尾に待機時間やコールバックを追加します。
    //Join 直前のTweenと並行して動作するようにTween追加を行います。
    // Joinの後Appendした場合もっとも時間のかかったTweenの後に実行されます。
    //Insert / InsertCallback
    //Prepend / PrependCallback / PrependInterval
    //Sequence同士をSequenceで繋ぐ
}

