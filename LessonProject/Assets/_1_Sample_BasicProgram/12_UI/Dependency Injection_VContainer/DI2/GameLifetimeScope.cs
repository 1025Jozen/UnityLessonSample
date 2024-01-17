using VContainer;
using VContainer.Unity;
using VContainer3;

//このスクリプトをコンポーネントにつける
namespace VContainer2 {
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder) {
            
            builder.Register<GameManager>(Lifetime.Singleton); //シングルトン登録
            builder.Register<HelloService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameEntryPoint>();     //エントリーポイント登録
        }
    }
}