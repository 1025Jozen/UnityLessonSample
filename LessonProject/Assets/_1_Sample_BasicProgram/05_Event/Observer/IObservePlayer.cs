namespace Observer
{
    public interface IObservePlayer//Playerを観察するクラスにつける
    {
        public void OnPlayerNotify(PlayerActionStatus playerStatus);
        // プレイヤーが通知してくれる
        // プレイヤーが通知してくれると  プレイヤーの観察者で実行される（実行してもらう関数　イベント）　
        // 通知のパラメーターは場合によって異なる
    }
}