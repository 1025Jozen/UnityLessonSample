namespace VContainer2
{
    /// <summary>ゲーム管理クラス</summary>
    public class GameManager
    {
        readonly HelloService _helloService;


        public GameManager(HelloService helloService) {
            _helloService = helloService;
        }
        

        public void OnStart() {
            _helloService.Hello();
        }
        public void OnUpdate() {
        }
    }
}