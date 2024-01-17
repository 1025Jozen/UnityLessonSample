using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observer
{
    //UnityやC#の event　Actionを使ってもいいが、ここではそれらを使わない
    //Observerパターンでの実装
    //観察対象(通知者)
    public abstract class PlayerSubject : MonoBehaviour
    {
        private List<IObservePlayer> observePlayerList     = new List<IObservePlayer>();//今回はリストで観察者を把握
        //private HashSet<IObservePlayer> observers        = new HashSet<IObservePlayer>();
        //private Dictionary<int,IObservePlayer> observers = new Dictionary<int,IObservePlayer>();


        public void AddPlayerObserver(IObservePlayer observer) {
            observePlayerList.Add(observer);//実行するクラス(観察者)をリストに追加
        }

        public void RemovePlayerObserver(IObservePlayer observer) {
            observePlayerList.Remove(observer);
        }

        public void NotifyPlayerObserver(PlayerActionStatus playerStatus) {//観察者に通知　　メソッドを実際に実行する 継承先で呼び出したい部分に記述
            observePlayerList.ForEach((observer) => {
                observer.OnPlayerNotify(playerStatus);//クラスごとに登録してある処理を実行
            });
        }
    }
}