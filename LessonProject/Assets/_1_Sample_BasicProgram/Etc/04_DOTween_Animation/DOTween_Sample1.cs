using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DOTween_Sample1 {
    public class DOTween_Sample1 : MonoBehaviour
    {
        //Tween tween;

        //////メンバ変数でSequenceのインスタンスを作成
        //Sequence sequence = DOTween.Sequence();

        //[SerializeField] AudioSource audioSource;
        //[SerializeField] Renderer rendererComponent;

        Vector3[] path =
             {
                new Vector3(0f,0f,10f),
                new Vector3(5f,0f,10f),
                new Vector3(5f,0f,0f),
                new Vector3(0f,0f,0f)
            };


        void Start() {



            //// 3秒かけて(5,0,0)へ移動する
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f);

            ////(5,0,0)へ1秒で移動するのを3回繰り返す
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f).SetLoops(3, LoopType.Restart);

            ////3秒待ってから(5,0,0)へ1秒で移動する
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f).SetDelay(3f);

            ////(5,0,0)へ1秒でリニア移動する
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f).SetEase(Ease.Linear);

            ////2秒待ってから(5,0,0)へ 3秒で移動するのを4回(2往復)  OutBounceで行う
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f)
            //                .SetDelay(2f)
            //                .SetLoops(4, LoopType.Yoyo)
            //                .SetEase(Ease.OutBounce);

            //this.tween = this.transform.DOMove(new Vector3(5f, 0f, 0f), 2f).SetLoops(-1, LoopType.Yoyo);

            ////(5,0,0)に2秒で移動し、移動が完了したらY軸180度に1秒で回転する
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 2f).OnComplete(() =>
            //{
            //    this.transform.DORotate(Vector3.up * 180f, 1f);
            //});

            //Debug.Log(Time.realtimeSinceStartup);

            ////(5,0,0)に2秒で移動し、移動開始時にログを出す
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 2f).OnStart(() =>
            //{
            //    Debug.Log("OnStart" + Time.realtimeSinceStartup);
            //}).SetDelay(2f);

            ////(5,0,0)に1秒で移動し、移動中にログを出す
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f).OnUpdate(() =>
            //{
            //    Debug.Log($"[{Time.frameCount}] OnUpdate {this.transform.position}");
            //});

            ////(5,0,0)に10秒で移動し、Killされたらログを出す
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f).OnKill(() =>
            //{
            //    Debug.Log("OnKill" + transform.position);
            //});

            ////(5,0,0)に10秒で移動し、Killされたらログを出す
            //this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f)
            //    .OnKill(() => {
            //        Debug.Log("OnKill" + this.transform.position);
            //    })
            //    .OnComplete(() => {
            //        Debug.Log("OnComplete" + this.transform.position);
            //    });


            ////Appendで動作を追加していく
            //sequence.Append(this.transform.DOMoveX(5f, 2f));
            //sequence.Append(this.transform.DOMoveY(2f, 1f));


            ////Joinはひとつ前の動作と同時に実行される
            //sequence.Join(this.transform.DOScale(Vector3.one * 2f, 1f));

            ////AppendIntervalで途中に待機を入れられる
            //sequence.AppendInterval(3f);


            ////Playで実行
            //sequence.Play();

            ////Playで実行
            //sequence.Play()
            //    .OnStart(() => {
            //        //開始時に呼ばれる
            //        Debug.Log("OnStart");
            //    })
            //    .OnUpdate(() => {
            //        //実行中に毎フレーム呼ばれる
            //        Debug.Log("OnUpdate");
            //    })
            //    .OnComplete(() => {
            //        //完了時に呼ばれる
            //        Debug.Log("OnComplete");
            //    })
            //    .OnKill(() => {
            //        //Kill時に呼ばれる
            //        Debug.Log("OnKill");
            //    });


            //指定したPathを10秒で通り、進行方向を向く
            transform.DOPath(path, 10f)
                .SetLookAt(0.01f)
                .SetLink(gameObject);//GameObjectがDestroyされると一緒にTween処理も停止されるようになり、警告も出ないようになります。
                                     //.OnComplete(()=> Destroy(gameObject)); ;     



            ////1秒でAudioSourceのVolumeを0にする
            //this.audioSource.DOFade(endValue: 0f, duration: 1f);

            ////1秒で赤色に変える
            //this.rendererComponent.material.DOColor(Color.red, 1f);
        }




        void Update() {
            //if (Input.GetKeyDown(KeyCode.A)) {
            //    //返り値を保存しておいて止める方法
            //    this.tween.Kill();
            //}

            //if (Input.GetKeyDown(KeyCode.S)) {
            //    //参照元を指定して止める方法
            //    this.transform.DOKill();
            //}

            //if (Input.GetKeyDown(KeyCode.D)) {
            //    //Objectを指定して止める方法
            //    DOTween.Kill(this.transform);
            //}

            //if (Input.GetKeyDown(KeyCode.F)) {
            //    //全ての実行を止める方法
            //    DOTween.KillAll();
            //}
            //if (Input.GetKeyDown(KeyCode.G)) {
            //    this.sequence.Kill();
            //}  
        }
    }
}

