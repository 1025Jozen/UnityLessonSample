using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

namespace Othello
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public static readonly int x_Row = 8;//x座標　列数　縦
        public static readonly int z_Col = 8;//z座標　行数　日本語だと行の数え方が縦も横もあるのでややこしい


        [SerializeField] Disk _diskPrefab;
        [SerializeField] Transform _diskBase;

        [SerializeField] PlayerSelf _playerSelf;
        [SerializeField] PlayerEnemy _playerEnemy;

        [SerializeField] GameObject _cursor;
        public GameObject Cursor => _cursor;

        [SerializeField] TextMeshProUGUI _resultText;

        float disk_Interval = 1f;

        public Disk[,] _disks { get; private set; }

        public enum GameState
        {
            None,
            Initializing,
            BlackTurn,
            WhiteTurn,
            Result
        }
        public GameState _gameState { get; private set; } = GameState.None;

        public int CurrentTurnCount
        {
            get
            {
                var turnCount = 0;
                for (var z = 0; z < z_Col; z++) {
                    for (var x = 0; x < x_Row; x++) {
                        // 盤面全部　を見て　そこからターン数を計算
                        if (_disks[z, x]._diskState != Disk.DiskState.None) {//diskが出現後なら
                            turnCount++;//ターン数を計算
                        }
                    }
                }
                return turnCount;
            }
        }


        void Start() {
            _disks = new Disk[z_Col, x_Row];//多次元配列
            for (var z = 0; z < z_Col; z++) {
                for (var x = 0; x < x_Row; x++) {
                    var disk = Instantiate(_diskPrefab, _diskBase);//diskBase基準に生成 //初めに生成しておく
                    var temp = disk.transform;//Transformのアドレス情報をtempに渡す
                    temp.localPosition = new Vector3(disk_Interval * x, 0, disk_Interval * z);
                    _disks[z, x] = disk;//disk情報を配列に入れる
                }
            }
            _gameState = GameState.Initializing;//ゲームのStateを初期状態にする
        }

        void Update() {
            //Debug.Log($"現在のゲームの状態　：　{gameState}");
            switch (_gameState) {
                case GameState.Initializing: {
                        _resultText.gameObject.SetActive(false);
                        for (var z = 0; z < z_Col; z++) {
                            for (var x = 0; x < x_Row; x++) {
                                //石を全部見えなくする
                                //_disks[z,x]　Diskクラスで石を見えなくする処理を書く
                                _disks[z, x].SetActive(false, Disk.DiskColor.Black);
                            }
                        }
                        //真ん中４つを見えるようにする  （白と黒交互に表示）
                        _disks[3, 3].SetActive(true, Disk.DiskColor.Black);
                        _disks[4, 4].SetActive(true, Disk.DiskColor.Black);
                        _disks[3, 4].SetActive(true, Disk.DiskColor.White);
                        _disks[4, 3].SetActive(true, Disk.DiskColor.White);//白　
                        _gameState = GameState.BlackTurn;
                    }
                    break;
                case GameState.BlackTurn: {
                        if (IsAnimating()) {
                            break;
                        }
                        //石が置けるなら
                        //石を置く(置いた場所の石を表示)
                        //置いたらひっくり返す
                        if (_playerSelf.TryGetSelected(out var z, out var x)) {
                            _disks[z, x].SetActive(true, Disk.DiskColor.Black);//石を置く
                            Reverse(Disk.DiskColor.Black, z, x);//ひっくり返す
                                                                //石が置けるか判断
                            if (_playerEnemy.CanPut()) {//白い石が置けるなら白のターン
                                _gameState = GameState.WhiteTurn;
                            } else if (!_playerSelf.CanPut()) {//相手も自分も置けないなら結果表示 
                                _gameState = GameState.Result;
                            }
                            //それ以外なら黒のターン継続
                        }
                    }
                    break;
                case GameState.WhiteTurn: {
                        if (IsAnimating()) {
                            break;
                        }
                        //if (_playerEnemy.TryGetSelected(out var z, out var x))
                        //{
                        //    _disks[z, x].SetActive(true, Disk.DiskColor.White);//石を置く
                        //    Reverse(Disk.DiskColor.White, z, x);//ひっくり返す
                        //                                        //石が置けるか判断
                        //    if (_playerSelf.CanPut())
                        //    {//黒い石が置けるなら黒のターン
                        //        _gameState = GameState.BlackTurn;
                        //    }
                        //    else if (!_playerSelf.CanPut())
                        //    {//置けないなら結果表示
                        //        _gameState = GameState.Result;
                        //    }
                        //    //それ以外なら白のターン継続
                        //}

                        //置けるなら
                        if (_playerEnemy.TryGetSelected(out var z, out var x)) {
                            _disks[z, x].SetActive(true, Disk.DiskColor.White);

                            Reverse(Disk.DiskColor.White, z, x);//z,xの位置を白色を置いて該当箇所をひっくり返す

                            if (_playerSelf.CanPut()) {//黒い石が置けるなら黒のターンへ
                                _gameState = GameState.BlackTurn;
                            } else if (!_playerEnemy.CanPut()) {//両方置けないなら終了
                                _gameState = GameState.Result;
                            }
                            //CurrentState = State.BlackTurn;
                        }


                    }
                    break;
                case GameState.Result:
                    Debug.Log("結果表示"); {
                        if (!_resultText.gameObject.activeSelf) {
                            int blackScore;
                            int whiteScore;
                            CalcScore(out blackScore, out whiteScore);
                            _resultText.text = blackScore > whiteScore ? "You Win"
                                : blackScore < whiteScore ? "You Lose" : "Draw";
                            _resultText.gameObject.SetActive(true);
                            //詳細スコア表示　blackScore　whiteScore
                        }
                        if (Input.GetKeyDown(KeyCode.Return)) {
                            _gameState = GameState.Initializing;
                        }
                    }
                    break;
            }
        }

        private bool IsAnimating() {
            for (var z = 0; z < z_Col; z++) {
                for (var x = 0; x < x_Row; x++) {
                    switch (_disks[z, x]._diskState) {
                        case Disk.DiskState.Appearing:
                        case Disk.DiskState.Reversing:
                            return true;
                    }
                }
            }
            return false;
        }



        private void Reverse(Disk.DiskColor diskColor, int z_Put, int x_Put) {
            //8方向分（8箇所分の処理）
            //置いた位置の上方向　下方向 (配列の上と下を確認できるようにする)
            for (var z_Dir = 1; z_Dir >= -1; z_Dir--) {
                //置いた位置の右方向　左方向 (配列の上と下を確認できるようにする)
                for (var x_Dir = 1; x_Dir >= -1; x_Dir--) {
                    // １方向　の ひっくり返す事のできる数を計算
                    var reverseCount = CalcReverseCount(diskColor, z_Put, x_Put, z_Dir, x_Dir);
                    //ひっくり返せる数だけひっくり返す
                    for (var i = 1; i <= reverseCount; i++) {
                        //石　にひっくり返ってもらう
                        //Debug.Log($"z_Put + z_Dir * i {z_Put + z_Dir * i}");
                        //Debug.Log($"x_Put + x_Dir * i {x_Put + x_Dir * i}");
                        _disks[z_Put + z_Dir * i, x_Put + x_Dir * i].Reverse();//ひっくり返す
                    }
                }
            }
        }


        //操作している色 置いた位置　z座標とx座標　置いた位置から見た方向 
        int CalcReverseCount(Disk.DiskColor diskColor, int z_Put, int x_Put, int z_Dir, int x_Dir) {
            //Debug.Log($"置けるかどうか");
            var z = z_Put;//z_Put座標に関してのtempの変数
            var x = x_Put;//x_Put座標に関してのtempの変数
            var reverseTempCount = 0;//ひっくり返す数
            for (var i = 0; i < 8; i++) { //マジックナンバーよくない　ここでは8行8列　として　８で処理を回してる
                z += z_Dir;//z方向
                x += x_Dir;//x方向


                if (z < 0 || z >= z_Col || x < 0 || x >= x_Row) {//置けない位置まで行ったら　0を返す
                                                                 //Debug.Log($"置けない  {reverseTempCount}");
                    reverseTempCount = 0;
                    break;
                }
                //Debug.Log($"探る位置　{z} ,{x}");
                var disk = _disks[z, x];//石単体の情報
                if (disk._diskState == Disk.DiskState.None) {//表示されていないなら カウントしない
                                                             //Debug.Log($"表示されていない　{reverseTempCount}");
                    reverseTempCount = 0;
                    break;
                } else {//表示されていない以外
                    if (disk._diskColor != diskColor) {//操作している色と表示されている色が違うなら
                        reverseTempCount++;//カウント数をプラス
                                           //Debug.Log($"ひっくり返せる　{reverseTempCount}");
                    } else {
                        //Debug.Log($"表示されているうえ色が同じ　{reverseTempCount}");
                        break;
                    }//表示されているうえ色が同じなら
                }
            }
            return reverseTempCount;//最終的にひっくり返す石の数を返す
        }


        //ひっくり返せる場所があるか  
        public int CalcTotalRreverseCount(Disk.DiskColor color, int z_Put, int x_Put) {
            //Debug.Log($"置ける場所かどうか{_disks[z_Put, x_Put]._diskState}");
            //置く場所がNoneならそこは置ける場所（それ以外はすでに置いてある）
            if (_disks[z_Put, x_Put]._diskState != Disk.DiskState.None) return 0;
            var totalReverseCount = 0;
            for (var z_Dir = 1; z_Dir >= -1; z_Dir--) {
                for (var x_Dir = 1; x_Dir >= -1; x_Dir--) {
                    //Debug.Log($"x_Dir {x_Dir}");
                    totalReverseCount += CalcReverseCount(color, z_Put, x_Put, z_Dir, x_Dir);
                }
            }
            //Debug.Log($"ひっくり返せる数{totalReverseCount}");
            return totalReverseCount;//0以上なら置ける場所があるということ
        }

        // 配列内のState状態　黒と白　両方をカウント
        private void CalcScore(out int blackScore, out int whiteScore) {
            blackScore = 0;
            whiteScore = 0;
            for (var z = 0; z < z_Col; z++) {
                for (var x = 0; x < x_Row; x++) {
                    if (_disks[z, x]._diskState != Disk.DiskState.None) {
                        switch (_disks[z, x]._diskColor) {
                            case Disk.DiskColor.Black:
                                blackScore++;
                                break;
                            case Disk.DiskColor.White:
                                whiteScore++;
                                break;
                        }
                    }
                }
            }
        }



        //評価値計算するなら下の配列と見比べてスコアを出す
        //その評価の大きさによってEnemyにDiskを置かせるなどしても良い
        //minimax   negamax alpha-beta  nega-alpha
        //ゲーム状況によって途中からモンテカルロ法を利用するなどしてもいい
        //ML_Agentsで機械学習させた方法も試したい
        /// <summary>
        /// ストーン状態に対する評価値
        /// </summary>
        private static readonly int[,] EvaluateStoneStatesScore = new[,] {
            {  30, -12,   0,  -1,  -1,   0, -12,  30},
            { -12, -15,  -3,  -3,  -3,  -3, -15, -12},
            {   0,  -3,   0,  -1,  -1,   0,  -3,   0},
            {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1},
            {  -1,  -3,  -1,  -1,  -1,  -1,  -3,  -1},
            {   0,  -3,   0,  -1,  -1,   0,  -3,   0},
            { -12, -15,  -3,  -3,  -3,  -3, -15, -12},
            {  30, -12,   0,  -1,  -1,   0, -12,  30},
        };






    }
}


