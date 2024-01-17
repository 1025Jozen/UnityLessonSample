
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

//受信側
//CubeのGameObjectにアタッチ
namespace MessagePipeSample.InputProvider
{
    public sealed class MoveCube : MonoBehaviour
    {
        /// <summary> MessagePipeからメッセージを受け取る用インタフェース </summary>
        [Inject] private ISubscriber<InputParams> _inputEventSubscriber;//ISubscriber受け取り側  //[Inject] 

        // 各種フィールド
        private CharacterController _characterController;
        private readonly float JumpSpeed = 3.0f;
        private readonly float MoveSpeed = 3.0f;

        private void Start() {
            _characterController = GetComponent<CharacterController>();

            // 入力イベントの受信を開始する
            _inputEventSubscriber
                .Subscribe(OnInputEventReceived)             //受け取ったら  OnInputEventReceivedを実行  　
                .AddTo(this.GetCancellationTokenOnDestroy());// MonoBehaviourに寿命を紐づける（これはUniTaskの機能）
        }

        /// <summary> InputParamsを受け取って、入力イベントを処理する </summary>
        private void OnInputEventReceived(InputParams input) {
            var moveVelocity = new Vector3(0, _characterController.velocity.y, 0);

            if (input.IsJump && _characterController.isGrounded) {//ジャンプ入力　かつ　地面に着地していたら
                moveVelocity += Vector3.up * JumpSpeed;
            }

            moveVelocity += input.Move * MoveSpeed;

            moveVelocity += Physics.gravity * Time.deltaTime;//重力も計算

            _characterController.Move(moveVelocity * Time.deltaTime);//実際にキャラを動かす
        }
    }
}
