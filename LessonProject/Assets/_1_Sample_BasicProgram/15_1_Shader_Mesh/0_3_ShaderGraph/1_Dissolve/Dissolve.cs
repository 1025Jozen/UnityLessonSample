using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Dissolve_Sample {
    public class Dissolve : MonoBehaviour
    {
        [SerializeField] Renderer[] renderers = { };            //dissoveさせたい(Materialが設定されている)Render
        [SerializeField, Min(0)] float effectDuration = 1f;     //間隔
        [SerializeField] Ease effectEase = Ease.Linear;         //アニメーションの変位の仕方
        [SerializeField] string progressParamName = "_Progress";//ShaderGraphで設定した名前

        List<Material> materials = new List<Material>();//renderersからmaterialを取得してMaterialのプロパティの数値を変更予定
        Sequence sequence;//アニメーション

        void Start() {
            GetMaterials();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) {
                DissolveIn();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                DissolveOut();
            }
        }

        public void DissolveIn() {
            sequence = DOTween.Sequence().SetLink(gameObject).SetEase(effectEase);

            foreach (Material m in materials) {
                m.SetFloat(progressParamName, -0.1f);
                sequence.Join(m.DOFloat(1, progressParamName, effectDuration));
            }

            sequence.Play();
        }

        public void DissolveOut() {
            sequence = DOTween.Sequence().SetLink(gameObject).SetEase(effectEase);//アニメーション

            foreach (Material m in materials) {
                m.SetFloat(progressParamName, 1);//マテリアルのprogressプロパティに１を設定
                sequence.Join(m.DOFloat(-0.1f, progressParamName, effectDuration));//progressプロパティを第３引数の間隔でアニメーションさせる
            }

            sequence.Play();
        }

        void GetMaterials() {
            foreach (Renderer r in renderers) {
                foreach (Material m in r.materials) {
                    materials.Add(m);
                }
            }
        }
    }
}

