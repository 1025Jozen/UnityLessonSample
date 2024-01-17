using VContainer;
using VContainer.Unity;

namespace VContainer3
{
    //Presenter: ModelとViewの間でデータの受け渡しと制御を行う部分
    public class GamePresenter : IStartable
    {
        private readonly HelloService _helloService;
        private readonly GameView _gameView;

        [Inject]//コンストラクタに[Inject]を指定することで登録したクラスが渡される
        public GamePresenter(HelloService helloService, GameView gameView) {
            _helloService = helloService;
            _gameView     = gameView;
        }

        public void Start() {
            var message = _helloService.HelloString();//テキスト取得
            _gameView.SetSampleText(message);//表示
        }
    }
}