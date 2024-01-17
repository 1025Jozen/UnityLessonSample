using UnityEngine;

namespace Observer
{
    public class PlayerStateMachine : PlayerSubject //通知者継承　　//通知を受け取るリストをここで取得(継承)
    {
        [SerializeField] KeyCode _resetSceneKey = KeyCode.F;
        PlayerActionStatus currentPlayerActionStatus = PlayerActionStatus.die;

        private void Update() {
            if (Input.GetKeyDown(_resetSceneKey)) {
                NotifyPlayerObserver(currentPlayerActionStatus);//Observerに通知 　//リスト中身を見て　インターフェイスに書いた関数を実行
            }
        }
    }
}