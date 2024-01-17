using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MNIST
{
    public static class LinqExtensions
    {
        // IEnumerable  反復処理をサポートする列挙子。要素を順番に列挙するための機能を提供します。また、列挙（foreach）
        /// <summary> SoftMax関数※ softmax関数を通すと、全体を1としたときの0 ~ 1の確率の値になります </summary>


        public static IEnumerable<float> SoftMax(this IEnumerable<float> source) {
            //ソフトマックス関数という
            //n個のデータがあるときに、合計を1(100 %)になるように調整してくれる式があって、
            //その数式をプログラムにしたのがこれ
            var exp = source.Select(Mathf.Exp).ToArray();
            var sum = exp.Sum();
            return exp.Select(x => x / sum);
        }
    }
}