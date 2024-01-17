using System.Collections.Generic;
using UnityEngine;

namespace Command
{
    public class CommandInvoker//コマンド呼び出し元・コマンドに関するリスト
    {
        public List<ICommand> commandList = new List<ICommand>();//コマンドの保存stack　ここの操作をすることでUndoなども実装できるだろう

        private int maxSavedCommands = 10; // 保存できる最大要素数

        private List<ICommand> redoStack = new List<ICommand>();//元にもどす用






        //追加
        public void AddCommand(ICommand command) {
            commandList.Add(command);                   //リストに加える
            if (commandList.Count > maxSavedCommands) { //
                commandList.RemoveAt(0); // 保存要素数を超えたら一番古い要素を削除
            }
            if (redoStack.Count > maxSavedCommands) {
                redoStack.Clear();                         // 新しい操作が行われたら、redoStackをクリアする
            }
        }

        //コマンド実行
        public void ExecuteCommands() {
            if (commandList.Count > 0) {
                ICommand command = commandList[commandList.Count - 1];
                command.Execute();//ここが実行しているところ 関数増やすなら各クラスに実装(Interfaceにも実装しないと。。。)
                commandList.RemoveAt(commandList.Count - 1);
            }
        }


        ////再度同じ
        //public void UndoLastCommand() {
        //    if (commandList.Count > 0) {
        //        int lastIndex = commandList.Count - 1;//最後の要素番号
        //        ICommand lastCommand = commandList[lastIndex];//最後に実行したコマンドを取り出す

        //        commandList.RemoveAt(lastIndex);// 実行したコマンドリストの最後の要素を削除

        //        redoStack.Add(lastCommand);     // 操作をredoStackに保存
        //                                        // 最後に実行したコマンドを別のリストに加える。
        //                                        // （0が一番新しい　要素が増えていくごとに昔に実行したコマンドが保存される）

        //        lastCommand.Execute();          // 最後のコマンドを取り出して実行　　　　　　　
        //    }
        //}


        ////戻したけどやっぱり元に戻したい時 //　0が１番最後にした実行   
        //public void RedoLastCommand() {
        //    if (redoStack.Count > 0) {
        //        int lastIndex = redoStack.Count - 1;
        //        ICommand lastRedo = redoStack[lastIndex];//最後に実行したコマンドを取り出す

        //        redoStack.RemoveAt(lastIndex); // 最後に実行した
        //        commandList.Add(lastRedo);     // redoStackの操作をcommandListに戻す
        //        lastRedo.Execute();            // 最後に戻したコマンドを取り出して再度実行（やり直す）
        //    }
        //}




    }
}