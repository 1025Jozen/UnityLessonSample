using VContainer;
using VContainer.Unity;

namespace VContainer3
{
    /// <summary> エントリーポイント </summary>
    public class GameEntryPoint : IStartable, ITickable
    {
        readonly GameManager _gameManager;

        [Inject]
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



