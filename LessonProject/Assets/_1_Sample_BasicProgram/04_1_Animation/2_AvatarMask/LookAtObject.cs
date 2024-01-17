using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation_Sample {
    public class LookAtObject : MonoBehaviour
    {
        [SerializeField] GameObject targetObject;       // このObjectの方向を向いてもらう

        Animator animator;
        Vector3 targetPos;

        [SerializeField, Range(0, 1)] float lookAtWeight = 1.0f;
        [SerializeField, Range(0, 1)] float bodyWeight   = 0.3f;
        [SerializeField, Range(0, 1)] float headWeight   = 3f;
        [SerializeField, Range(0, 1)] float eyesWeight   = 0f;
        [SerializeField, Range(0, 1)] float clampWeight  = 0.5f;


        void Start() {
            this.animator = GetComponent<Animator>();
            this.targetPos = targetObject.transform.position;
        }

        void Update() {
            this.targetPos = targetObject.transform.position;
        }

        // OnAnimatorIKはIk Pass(Animatorウィンドウ　BaseLayer)にチェックを入れると使える
        private void OnAnimatorIK(int layerIndex) {
            // 体や頭、目の追従具合（重み）を調整するためのメソッド  どの部位がどのくらい見るかを決める　　　
            this.animator.SetLookAtWeight(lookAtWeight        , bodyWeight          , headWeight          , eyesWeight            , clampWeight);     // LookAtの調整
                                        //(全体の重み・全体の調整, 体を動かす重み・体の調整, 頭を動かす重み・頭の調整, 目を動かす重み・眼の調整, 可動域の調整・モーションの制限量)

            //(LookAtの方向)
            this.animator.SetLookAtPosition(this.targetPos);  // ターゲットの方向を向く
        }
    }
}

