using System.Collections.Generic;

namespace State
{
    public class StateFactory 
    {
        StateMachine currentStateMachine;
        Dictionary<PlayerAllStatus, StateBase> status = new Dictionary<PlayerAllStatus, StateBase>();//インスタンス辞書


        //操作する大元を引数にPlayerStateMachineでthisする　クラスは参照型 アドレスが入る
        //操作する大元のアドレス　と　各ステートを生成しているこのクラスのインスタンスアドレスを各ステートで参照できるようにする
        public StateFactory(StateMachine stateMachine) {//コンストラクタ　
            currentStateMachine = stateMachine;
            status[PlayerAllStatus.ground] = new PlayerGround(this, currentStateMachine);
            status[PlayerAllStatus.idle] = new PlayerIdle(this, currentStateMachine);
            status[PlayerAllStatus.walk] = new PlayerWalk(this, currentStateMachine);
        }


        // StateBaseを継承しておりクラスごとに状態を遷移させる。辞書登録の中から該当インスタンスを返している
        public StateBase Ground() {
            return status[PlayerAllStatus.ground];
        }

        public StateBase Idle() {
            return status[PlayerAllStatus.idle];
        }
        public StateBase Walk() {
            return status[PlayerAllStatus.walk];
        }
    }
}