using VContainer.Unity;

namespace VContainer1
{
    /// <summary> エントリーポイント </summary>
    public class GameEntryPoint : IStartable, ITickable//
    {
        private readonly GameManager _gameManager;

        public GameEntryPoint(GameManager gameManager) {//コンストラクタ
            _gameManager = gameManager;
        }
        public void Start() {//MonobehaviourでいうところのStart
            _gameManager.OnStart();
        }
        public void Tick() {//MonobehaviourでいうところのUpdateがTick
            _gameManager.OnUpdate();
        }


    }
}