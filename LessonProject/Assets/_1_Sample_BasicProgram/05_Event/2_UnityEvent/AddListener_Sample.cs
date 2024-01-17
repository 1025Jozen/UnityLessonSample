using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace UnityEvent_Sample
{
    public class AddListener_Sample : MonoBehaviour
    {
        [SerializeField] UnityEvent_Sample unityEvent_Sample;

        //private void Awake() {
        //    // Event に View 関数を渡す（引数が一致している必要がある）
        //    unityEvent_Sample.Event.AddListener(View);
        //}
        //private void View(Collision collision) {
        //    Debug.Log(collision.gameObject.name);
        //}

        private void Awake() {
            unityEvent_Sample.Event.AddListener((collision) =>{
                Debug.Log("AddListenr : " + collision.gameObject.name);
            });


            List<int> numbers = new List<int>();
            numbers.Add(1);
            Action action = () => Debug.Log(numbers[0]);
            numbers.Insert(0, 100);
            action.Invoke();// アクション実行時の状態による。今回は、コンソールに「100」と表示される
        }


    }
}