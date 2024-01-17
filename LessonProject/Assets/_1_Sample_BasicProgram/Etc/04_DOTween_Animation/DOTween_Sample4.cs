using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace DOTween_Sample {
    public class DOTween_Sample4 : MonoBehaviour
    {
        private void Update()
        {
            //transform.DOShakePosition(1f, 5f, 30, 1, false, true);
            //1   トゥイーン時間
            //2   振動する強さ
            //3   振動数
            //4   手ブレ値
            //5   スナップフラグ 
            //6   フェードアウト

            //transform.DOShakePosition(1f, 3f, 90, 30, false, false);

            transform.DOShakeRotation(1f, 90f, 30, 90, true);

            //transform.DOShakeScale(1f, 3f, 30, 90f, true);
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

