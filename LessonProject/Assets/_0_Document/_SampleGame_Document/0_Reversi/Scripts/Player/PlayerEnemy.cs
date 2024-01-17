using System;
using System.Linq;

namespace Othello
{
    public class PlayerEnemy : PlayerBase
    {
        public override Disk.DiskColor MyColor { get { return Disk.DiskColor.White; } }


        private Random _random = new Random();

        public override bool TryGetSelected(out int x, out int z) {
            var availablePoints = CalcAvailablePoints();//置ける場所ごとの情報

            var maxCount = availablePoints.Values.Max();//value要素の中で一番大きい値

            //value要素の中で一番大きいやつを探し出してきて　その大きい要素のkeyに当たるものを座標配列として取り出す
            var list = availablePoints.Where(p => p.Value == maxCount).Select(p => p.Key).ToList();

            if (list.Count > 0)//座標配列がちゃんとあれば
            {
                var point = list[_random.Next(list.Count)];//座標配列の中からランダムで要素を取り出す
                x = point.Item1;
                z = point.Item2;
                return true;
            } else {
                throw new Exception("Invalid State Enemy Cannot Put Stone");//配列に何もなければ、置けない
            }
        }






















        //人力　
        //private Vector3Int _cursorPos = Vector3Int.zero;//カーソル位置 初期化
        //private Vector3Int? _desidedPos = null;
        ////型の後ろに「?」を付ける事によって「nullを許容する」変数の性質を持つようになります

        ////null許容値型はValueプロパティとHasValueプロパティを持っています。
        ////Valueプロパティは値がnullでない場合の実際の値です。
        ////HasValueプロパティは値がnull以外ならば真となります。

        ////private void Start()
        ////{
        ////    int? num1 = 123;
        ////    int? num2 = null;

        ////    bool b1 = num1.HasValue; //true
        ////    bool b2 = num2.HasValue; //false

        ////    Console.WriteLine(num1.Value); //123
        ////    Console.WriteLine(num2.Value); //エラー
        ////}

        ////カーソル位置が値を持っているなら、　その値を位置座標として持ち、選択できるとする
        ////カーソル位置が値を持っていないなら、位置座標をリセットし、選択できないとする
        //public override bool TryGetSelected(out int z, out int x)
        //{
        //    if (_desidedPos.HasValue)
        //    {
        //        var pos = _desidedPos.Value;
        //        x = pos.x;
        //        z = pos.z;
        //        return true;
        //    }
        //    x = 0;
        //    z = 0;
        //    return false;
        //}

        //private bool TryCursorMove(int deltaX, int deltaZ)
        //{
        //    var z = _cursorPos.z;//カーソルの位置を一時的に保存
        //    var x = _cursorPos.x;

        //    z += deltaZ;
        //    x += deltaX;
        //    //制限
        //    if (z < 0 || GameManager.z_Col <= z)
        //        return false;
        //    if (x < 0 || GameManager.x_Row <= x)
        //        return false;

        //    _cursorPos.z = z;
        //    _cursorPos.x = x;

        //    GameManager.Instance.Cursor.transform.localPosition = _cursorPos * 1;
        //    return true;
        //}
        //private int _processingPlayerTurn = 0;
        //private void ExecTurn()
        //{//ターンを実行

        //    var turnCount = GameManager.Instance.CurrentTurnCount;
        //    if (_processingPlayerTurn != turnCount)
        //    {//ターンが被ってないなら
        //     //カーソルの色変更も必要　//カーソルにも意志が必要になるかもしれないが、今回はスルー
        //        GameManager.Instance.Cursor.SetActive(true);//カーソルが動くようにしてみる
        //        _desidedPos = null;//決定位置の初期化
        //        _processingPlayerTurn = turnCount;//ターン数を保持し直す
        //    }
        //    if (Input.GetKeyDown("d"))
        //    {
        //        TryCursorMove(1, 0);
        //    }
        //    else if (Input.GetKeyDown("a"))
        //    {
        //        TryCursorMove(-1, 0);
        //    }
        //    else if (Input.GetKeyDown("w"))
        //    {
        //        TryCursorMove(0, 1);
        //    }
        //    else if (Input.GetKeyDown("s"))
        //    {
        //        TryCursorMove(0, -1);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        //もしひっくり返せる数が０以上なら
        //        //カーソルの位置を　決定位置にして
        //        //カーソルを消す
        //        //Debug.Log(GameManager.Instance.CalcTotalRreverseCount(MyColor, _cursorPos.x, _cursorPos.z));
        //        if (GameManager.Instance.CalcTotalRreverseCount(MyColor, _cursorPos.z, _cursorPos.x) > 0)
        //        {
        //            //Debug.Log($"置いた位置{z_Put}　{x_Put}");
        //            _desidedPos = _cursorPos;
        //            GameManager.Instance.Cursor.SetActive(false);
        //        }
        //    }
        //}

        //private void Update()
        //{
        //    //ひとまず何の制限もなくカーソルが動くようにしてみる
        //    switch (GameManager.Instance._gameState)
        //    {
        //        case GameManager.GameState.WhiteTurn:
        //            ExecTurn();
        //            break;
        //        default:
        //            break;
        //    }
        //}




    }
}