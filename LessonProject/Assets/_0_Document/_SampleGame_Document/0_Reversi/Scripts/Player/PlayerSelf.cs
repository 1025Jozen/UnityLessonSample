using UnityEngine;

namespace Othello
{
    public class PlayerSelf : PlayerBase
    {

        ////今回はPlayer　は　黒で固定　ネット対戦などするなら変える必要あり
        public override Disk.DiskColor MyColor { get { return Disk.DiskColor.Black; } }
        //private Vector3Int _cursorPos = Vector3Int.zero;//カーソル位置 初期化

        private Vector3Int _cursorPos = new Vector3Int(0, 0, 0);//カーソル位置 初期化
        private Vector3Int? _desidedPos = null;
        //型の後ろに「?」を付ける事によって「nullを許容する」変数の性質を持つようになります

        //null許容値型はValueプロパティとHasValueプロパティを持っています。
        //Valueプロパティは値がnullでない場合の実際の値です。
        //HasValueプロパティは値がnull以外ならば真となります。

        //private void Start()
        //{
        //    int? num1 = 123;
        //    int? num2 = null;

        //    bool b1 = num1.HasValue; //true
        //    bool b2 = num2.HasValue; //false

        //    Console.WriteLine(num1.Value); //123
        //    Console.WriteLine(num2.Value); //エラー
        //}

        //カーソル位置が値を持っているなら、　その値を位置座標として持ち、選択できるとする
        //カーソル位置が値を持っていないなら、位置座標をリセットし、選択できないとする
        public override bool TryGetSelected(out int z, out int x) {
            if (_desidedPos.HasValue) {
                var pos = _desidedPos.Value;
                x = pos.x;
                z = pos.z;
                return true;
            }
            x = 0;
            z = 0;
            return false;
        }

        private bool TryCursorMove(int deltaX, int deltaZ) {
            var z = _cursorPos.z;//カーソルの位置を一時的に保存
            var x = _cursorPos.x;

            z += deltaZ;
            x += deltaX;
            //制限
            if (z < 0 || GameManager.z_Col <= z)
                return false;
            if (x < 0 || GameManager.x_Row <= x)
                return false;

            _cursorPos.z = z;
            _cursorPos.x = x;
            //Vector3 _cursorPos3 =_cursorPos ;
            GameManager.Instance.Cursor.transform.localPosition = _cursorPos * 1;//+ new Vector3(0.5f,0f,0.5f);
            return true;
        }
        private int _processingPlayerTurn = 0;
        private void ExecTurn() {//ターンを実行

            var turnCount = GameManager.Instance.CurrentTurnCount;
            if (_processingPlayerTurn != turnCount) {//ターンが被ってないなら
                ShowDots();
                GameManager.Instance.Cursor.SetActive(true);//カーソルが動くようにしてみる
                _desidedPos = null;//決定位置の初期化
                _processingPlayerTurn = turnCount;//ターン数を保持し直す
            }
            if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow)) {
                TryCursorMove(1, 0);
            } else if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow)) {
                TryCursorMove(-1, 0);
            } else if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow)) {
                TryCursorMove(0, 1);
            } else if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow)) {
                TryCursorMove(0, -1);
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                //もしひっくり返せる数が０以上なら
                //カーソルの位置を　決定位置にして
                //カーソルを消す
                //Debug.Log(GameManager.Instance.CalcTotalRreverseCount(MyColor, _cursorPos.x, _cursorPos.z));
                if (GameManager.Instance.CalcTotalRreverseCount(MyColor, _cursorPos.z, _cursorPos.x) > 0) {
                    //Debug.Log($"置いた位置{z_Put}　{x_Put}");
                    _desidedPos = _cursorPos;
                    GameManager.Instance.Cursor.SetActive(false);
                    HideDots();
                }
            }
        }

        private void Update() {
            //ひとまず何の制限もなくカーソルが動くようにしてみる
            switch (GameManager.Instance._gameState) {
                case GameManager.GameState.BlackTurn:
                    ExecTurn();
                    break;
                default:
                    break;
            }
        }


        private void ShowDots() {
            var availablePoints = CalcAvailablePoints();
            var stones = GameManager.Instance._disks;
            foreach (var availablePoint in availablePoints.Keys) {
                var x = availablePoint.Item2;
                var z = availablePoint.Item1;
                stones[z, x].EnableDot();
            }
        }

        private void HideDots() {
            var stones = GameManager.Instance._disks;
            for (var z = 0; z < GameManager.z_Col; z++) {
                for (var x = 0; x < GameManager.x_Row; x++) {
                    if (stones[z, x]._diskState == Disk.DiskState.None) {

                        stones[z, x]
                            //.DisableDot();
                            .SetActive(false, Disk.DiskColor.Black);
                    }
                }
            }
        }

    }
}