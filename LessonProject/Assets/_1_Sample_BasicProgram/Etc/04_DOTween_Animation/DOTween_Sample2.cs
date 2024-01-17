using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DOTween_Sample {
    public class DOTween_Sample2 : MonoBehaviour
    {
        [SerializeField] private GameObject _completeObject;

        [SerializeField]Text _text;

        private void Start()
        {
            _text.DOCounter(0, 9999, 10f, true)
                .OnComplete(MyCompleteFunction)
                .SetLink(gameObject);
        }

        private void MyCompleteFunction() {
            _completeObject.SetActive(true);
        }

    }
}

