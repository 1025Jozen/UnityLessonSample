using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command
{
    //実行してもらうことだけ定義　
    public interface ICommand
    {
        void Execute();
    }
}