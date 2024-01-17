using VContainer;
using VContainer.Unity;

//このスクリプトをコンポーネントにつける
namespace VContainer1 {
    public class GameLifetimeScope : LifetimeScope //これが1つのコンテナ
    {
        protected override void Configure(IContainerBuilder builder) {//設定
            
            builder.Register<GameManager>(Lifetime.Singleton);//シングルトン登録      コンテナにGameManagerクラスを登録　インスタンス化される         
            builder.RegisterEntryPoint<GameEntryPoint>();     //エントリーポイント登録 GameEntryPointを動かす

        }
    }
}