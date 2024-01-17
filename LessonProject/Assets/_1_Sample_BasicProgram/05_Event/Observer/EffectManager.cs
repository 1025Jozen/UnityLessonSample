using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observer
{
    public class PlayerEffectManager : MonoBehaviour, IObservePlayer
    {
        [SerializeField] PlayerStateMachine playerStateMachine;
        [SerializeField] ParticleSystem particlePrefab;
        ParticleSystem effect;

        Dictionary<PlayerActionStatus, System.Action> effectHandlers;

        /*  */
        private void Awake() {
            //辞書型で実行Actionを所持　キーのタイプによって実行するActionを設定
            effectHandlers = new Dictionary<PlayerActionStatus, System.Action>() {
                { PlayerActionStatus.die, CreateEffect},
            };
        }
        private void Start() {
            effect = Instantiate(particlePrefab);
            effect.transform.position = this.transform.position;//出現させる場所
            effect.transform.parent   = this.transform;         //親子関係
        }


        /* IObservePlayerに必要なもの */
        private void OnEnable() {
            playerStateMachine.AddPlayerObserver(this);//このクラスがPlayerの状態を観察していることを示す（リストに追加）
        }
        private void OnDisable() {
            playerStateMachine.RemovePlayerObserver(this);
        }

        public void OnPlayerNotify(PlayerActionStatus playerStatus) {//プレイヤーからの通知が来たら実行してもらう
            switch (playerStatus) {
                case (PlayerActionStatus.die):
                    if (effectHandlers.ContainsKey(playerStatus)) {//辞書にキーが含まれてたら
                        effectHandlers[playerStatus]();//該当アクションを実行
                    }
                    return;
                default:
                    return;
            }
        }

        /*  */
        void CreateEffect() {            
            StartCoroutine(PlayEffect());//0.5秒したら止める
        }
        IEnumerator PlayEffect() {
            effect.Play();//エフェクトを再生
            yield return new WaitForSeconds(0.5f);
            effect.Stop();
        }


    }
}

