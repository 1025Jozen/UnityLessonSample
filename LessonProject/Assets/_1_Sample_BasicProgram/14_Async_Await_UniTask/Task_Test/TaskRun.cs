using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AsyncSample {
    public class TaskRun : MonoBehaviour
    {
        //void Awake() {
        //    test();//test() は A の世界線
        //}
        //void test() {
        //    Task.Run(() => longTimeDelay());
        //    //Task.Run は最も簡単なスレッド生成方法。
        //    //longTimeDelay() は B の世界線
        //}
        //void longTimeDelay() {            
        //    System.Threading.Thread.Sleep(3000);// 3 秒かかる重い処理
        //    Debug.Log("finished.");
        //}



        //void Awake() {
        //    test();//test() は A の世界線
        //}
        //void test() {
        //    Task.Run(() => longTimeDelay());
        //    //Task.Run は最も簡単なスレッド生成方法。
        //    //longTimeDelay() は B の世界線
        //}
        //void longTimeDelay() {
        //    // ×××  UnityEngine は　　ほぼ使えない
        //    UnityEngine.Random.Range(1, 10);    //ここで処理が止まり、これ以降実行されない
        //    System.Threading.Thread.Sleep(3000);// 3 秒かかる重い処理
        //    Debug.Log("finished.");
        //}


        //UnityEngineを無理やり使う
        //System.Threading.SynchronizationContext context;
        //void Awake() {
        //    context = System.Threading.SynchronizationContext.Current;
        //    test();
        //}
        //void test() {
        //    Task.Run(() => longTimeDelay());
        //}
        //void longTimeDelay() {
        //    int val = 0;
        //    context.Post(_ => { val = UnityEngine.Random.Range(1, 10); }, null);//スレッド(Bの世界)からAの世界を呼び出す           
        //    System.Threading.Thread.Sleep(3000);// 3 秒待つ
        //    Debug.Log($"finished. val={val}");
        //}



        ////戻り値が欲しければ
        //void Awake() {
        //    var _ = test();
        //}
        //async Task test() {
        //    int val = await Task.Run(() => longTimeDelay());//longTimeDelay()が実行されるのを待つ
        //    Debug.Log($"finished. val={val}");//約３秒後　
        //}
        //int longTimeDelay() {
        //    System.Random rand = new System.Random();
        //    int val = rand.Next(1, 10);
        //    // 3 秒待つ
        //    System.Threading.Thread.Sleep(3000);
        //    return val;
        //}





        //void Awake() {
        //    test();
        //}
        //async void test() {
        //    await test1();//test1を待って　３秒待って
        //    test2();      //async void なので await をつけられない。処理も待ってくれない
        //    await test3();
        //}
        //async Task test1() {
        //    await Task.Run(() => longTimeDelay("test1"));
        //}
        //async void test2() {
        //    await Task.Run(() => longTimeDelay("test2"));
        //}
        //async Task test3() {
        //    await Task.Run(() => longTimeDelay("test3"));
        //}
        //void longTimeDelay(string name) {
        //    Debug.Log($"start. name={name}");
        //    System.Threading.Thread.Sleep(3000);
        //    Debug.Log($"end. name={name}");
        //}









    }
}

