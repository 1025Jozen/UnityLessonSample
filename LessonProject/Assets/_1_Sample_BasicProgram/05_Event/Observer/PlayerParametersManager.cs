using UnityEngine;

namespace Observer
{
    public class PlayerParametersManager : MonoBehaviour, IObservePlayer
    {
        [SerializeField] PlayerStateMachine playerStateMachine;

        /*  */
        private void OnEnable() {
            playerStateMachine.AddPlayerObserver(this);//このクラスがPlayerの状態を観察していることを示す（リストに追加）
        }
        private void OnDisable() {
            playerStateMachine.RemovePlayerObserver(this);
        }

        /*  */
        public void OnPlayerNotify(PlayerActionStatus playerStatus) {//プレイヤーからの通知が来たら実行してもらう
            Debug.Log("通知を受け取り実行");
            //例えば攻撃を受けたら enumのdamage　や敵のstatusなどのパラメーターから　playerのHPを減らす
        }
    }
}