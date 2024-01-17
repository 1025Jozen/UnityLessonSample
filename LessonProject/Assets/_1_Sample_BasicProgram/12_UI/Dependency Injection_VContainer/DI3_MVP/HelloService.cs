using UnityEngine;

namespace VContainer3
{
    public class HelloService
    {
        public void Hello() {
            Debug.Log("Hello world");
            
        }

        //Model: データを保持する部分
        public string HelloString() {
            return "Hello world";
        }
    }
}