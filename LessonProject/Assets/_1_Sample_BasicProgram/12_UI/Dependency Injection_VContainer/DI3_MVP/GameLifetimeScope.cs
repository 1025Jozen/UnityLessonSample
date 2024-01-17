using UnityEngine;
using VContainer;
using VContainer.Unity;

//このスクリプトをコンポーネントにつける
namespace VContainer3 {
    public class GameLifetimeScope : LifetimeScope
    {

        [SerializeField] private GameView gameView;

        //DIコンテナに登録していく  (コンテナ内で一括してアドレスなどを参照せきる)
        protected override void Configure(IContainerBuilder builder) {

            builder.Register<GameManager>(Lifetime.Singleton);//シングルトン登録 //Register通常のサービスや実装の登録に使用
            builder.Register<HelloService>(Lifetime.Singleton);


            //特定のアドレスを登録
            builder.RegisterComponent(gameView); // コンポーネント登録(実体の登録) //RegisterComponentコンポーネントや特定のオブジェクトの登録に使用

            //使用するクラス
            builder.RegisterEntryPoint<GameEntryPoint>();//エントリーポイント登録。一般的にアプリケーションの起動時に呼び出される場所
            builder.RegisterEntryPoint<GamePresenter>(); //複数登録できる 
        }
    }
}