using System;
using System.Collections;
using System.Collections.Generic;
//using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Othello
{
    public abstract class PlayerBase : MonoBehaviour
    {
        //操作する色　自分の色
        public abstract Disk.DiskColor MyColor { get; }

        public virtual bool TryGetSelected(out int z, out int x) {//置く位置を引数にし、実際に置けるかどうかを返す
            z = 0;
            x = 0;
            return false;
        }

        public bool CanPut() {
            //置けるところが１ヶ所以上ならtrue
            return CalcAvailablePoints().Count > 0;
        }

        /// <summary>
        ///  置ける場所ごとの情報 (置ける場所がある時だけ情報が入ってる)　辞書型 
        /// </summary>
        /// <returns> 置ける場所ごとの情報　辞書型 Dictionary< Tuple<int z, int x> , int ひっくり返せる数> </returns>
        public Dictionary<Tuple<int, int>, int> CalcAvailablePoints() {
            var game = GameManager.Instance;
            var disks = game._disks;
            var availablePoints = new Dictionary<Tuple<int, int>, int>();
            //{ { key1, value1}, { key2, value2}, ... };
            //{ {(z,x), value1}, {(z,x), value2}, ... };


            for (var z = 0; z < GameManager.z_Col; z++) {
                for (var x = 0; x < GameManager.x_Row; x++) {
                    //8*8全部の石について
                    //石の状態がNone(置ける箇所)なら
                    if (disks[z, x]._diskState == Disk.DiskState.None) {
                        //ひっくり返せる場所があるか 
                        var reverseCount = game.CalcTotalRreverseCount(MyColor, z, x);
                        //ひっくり返せる場所があるなら 
                        if (reverseCount > 0) {
                            //{ {(z,x), reverseCount}, {(z,x), value2}, ... };
                            availablePoints[Tuple.Create(z, x)] = reverseCount;
                        }
                    }
                }
            }
            return availablePoints;
        }
    }

}



