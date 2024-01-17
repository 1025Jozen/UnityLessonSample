using System;
using UnityEngine;


//送るメッセージ内容

//InputParamsという構造体を定義して、これをメッセージとして送信します。


namespace MessagePipeSample.InputProvider
{
    /// <summary> ブロードキャストするメッセージ 入力情報 </summary>
    public readonly struct InputParams //: IEquatable<InputParams>//InputParams　クラスをインスタンス化するとき　引数で値を渡すが、その渡した値が　同じかどうか　同じクラスで　同じ引数か判断したい時に使用　IEquatable
    {
        /// <summary>ジャンプフラグ</summary>
        public bool IsJump { get; }

        /// <summary>移動操作</summary>
        public Vector3 Move { get; }

        //コンストラクタ　構造体
        public InputParams(bool isJump, Vector3 move) {
            IsJump = isJump;
            Move   = move;
        }



        //public bool Equals(InputParams other) {
        //    //ジャンプ中　かつ　動く方向が同じ ならtrue
        //    return IsJump == other.IsJump && Move.Equals(other.Move);//VEctor3.Equals(  ) 
        //}

        //public override bool Equals(object obj) {//Objectを引数にした場合
        //    return obj is InputParams other && Equals(other);//obj が InputParamsクラス に キャスト　キャスト結果をotherに入れる   && InputParams.Equals(  )
        //}

        ////HashCode 構造体を使用して、複数の値（構造体やクラスのフィールドなど）を 1 つのハッシュ コードに結合できます
        //public override int GetHashCode() {
        //    return HashCode.Combine(IsJump, Move);
        //}
    }
}
