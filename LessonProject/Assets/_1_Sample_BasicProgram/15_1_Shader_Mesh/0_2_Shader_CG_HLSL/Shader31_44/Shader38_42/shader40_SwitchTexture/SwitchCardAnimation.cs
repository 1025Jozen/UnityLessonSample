using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Shader_Sample {
    /// <summary>
    /// カードの絵柄が変わるDoTweenアニメーション
    /// </summary>
    public class SwitchCardAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject card;

        void Start() {
            var m = card.GetComponent<MeshRenderer>().material;
            var t = card.transform;
            var propId = Shader.PropertyToID("_RenderSwitch");

            DOTween.Sequence()
                .Append(t.DOLocalRotate(new Vector3(0, 90, 0), 0.25f))
                .Append(m.DOFloat(1, propId, 0)) //90度回転後、カードが切り替わる
                .Append(t.DOLocalRotate(new Vector3(0, 180, 0), 0.25f))//そのまま180度まで回転
                .Join(t.DOPunchScale(t.localScale * 0.15f, 0.2f, 1))
                .Play();
        }
    }
}

