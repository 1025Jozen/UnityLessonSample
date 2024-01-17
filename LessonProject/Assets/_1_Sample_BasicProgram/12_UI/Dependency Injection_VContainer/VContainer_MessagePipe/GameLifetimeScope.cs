using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

//DIの設定登録  としてUnityのObjectにアタッチ
namespace MessagePipeSample.InputProvider
{
    public class GameLifetimeScope : LifetimeScope
    {
        // MoveCubeのPrefabへの参照
        [SerializeField] private MoveCube _moveCubePrefab;

        protected override void Configure(IContainerBuilder builder) {//設定     
            var options = builder.RegisterMessagePipe();// MessagePipeの設定            
            builder.RegisterMessageBroker<InputParams>(options);               // MessageBrokerを通して伝達するという設定　// InputParamsを伝達できるように設定する            
            builder.RegisterEntryPoint<InputEventProvider>(Lifetime.Singleton);// InputEventProviderクラスのインスタンスをシングルトンで設定　//ITickableを継承しているとUpdate関数としてTickメソッドが動く

            // MoveCubeをDIしながらInstantiate
            builder.RegisterBuildCallback(resolver => {
                resolver.Instantiate(_moveCubePrefab);


            });//resolver　を利用して　Instantiateを呼び出し、インスタンスを生成　
        }
    }
}
