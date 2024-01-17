using MessagePipe;
using UnityEngine;
using VContainer.Unity;

//送信側
namespace MessagePipeSample.InputProvider
{
    public sealed class InputEventProvider : ITickable
    {

        //単一のMessageBrokerを全員で共有する
        //IPublisher に色々登録  →　　(MessageBroker)　 →　ISubscriber　で　呼び出し　　　  IPublisher< TKey , T> などでKey登録もできる
        /// <summary> MessagePipeにメッセージを流す用のインタフェース </summary>
        private readonly IPublisher<InputParams> _inputPublisher;

        //コンストラクタ　インスタンス設定
        public InputEventProvider(IPublisher<InputParams> inputPublisher) {
            _inputPublisher = inputPublisher;
        }

        // 毎フレーム実行　LifetimeScope継承クラスで　RegisterEntryPointすることで　ITickable　を継承していることにより毎フレーム動く
        public void Tick() {
            // 入力状態を監視
            var isJump = Input.GetKey(KeyCode.Space);
            var axis   = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            
            var inputParams = new InputParams(isJump, axis);// メッセージを作成

            
            _inputPublisher.Publish(inputParams);// メッセージ送信  (MessageBroker経由でIPublisherが反応)
        }
    }
}
