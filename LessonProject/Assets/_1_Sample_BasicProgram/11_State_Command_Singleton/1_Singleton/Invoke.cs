using UnityEngine;
using UnityEngine.SceneManagement;

namespace Singleton_Sample
{
    public class Invoke : MonoBehaviour
    {
        int b;
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                SceneManager.LoadScene("Singleton2");
            }

            //Wを押した後、EとRを押したら何と出力される？
            //Qを押した後、EとRを押したら何と出力される？
            if (Input.GetKeyDown(KeyCode.W)) {
                SingletonSample.instance.test0();// SingletonSampleクラスのインスタンス内の変数の値を増やす
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                SingletonSample.instance.test1(out int a);//in／out／refは参照渡し(必要になったら違いを調べて)　
                Debug.Log($"test1: {a}"); 　　　　　　　　　//アドレスの値を変更
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                SingletonSample.instance.test2(b);// 値渡し 値を渡すだけ　
                Debug.Log($"test2: {b}");　　　　　//大元(アドレス元)は変わってないので
            }
        }
    }
}
