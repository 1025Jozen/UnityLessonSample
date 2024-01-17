using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command
{
    public class Client : MonoBehaviour
    {
        //[SerializeField] KeyCode upKey    = KeyCode.UpArrow;//実行
        //[SerializeField] KeyCode downKey  = KeyCode.DownArrow;//実行
        //[SerializeField] KeyCode rightKey = KeyCode.RightArrow;//実行
        //[SerializeField] KeyCode leftKey  = KeyCode.LeftArrow;//実行

        [SerializeField] KeyCode undoKey = KeyCode.R;  //元に戻す
        [SerializeField] KeyCode redoKey = KeyCode.Y;  //やり直す

        CommandInvoker commandInvoker;//クラス内にコマンドを保持するstack　< IComand > がある
        PlayerMove player;

        void Start() {
            player         = new PlayerMove();         //プレイヤーの動き　インスタンス化
            commandInvoker = new CommandInvoker(); //
        }
        void Update() {
            //if (Input.GetKeyDown(upKey)) {
            //    commandInvoker.AddCommand(new MoveUpCommand(player));//実行する(した)コマンドリスト
            //    commandInvoker.ExecuteCommands();// コマンドの実行
            //}
            //if (Input.GetKeyDown(downKey)) {
            //    commandInvoker.AddCommand(new MoveDownCommand(player));
            //    commandInvoker.ExecuteCommands();
            //}
            //if (Input.GetKeyDown(rightKey)) {
            //    commandInvoker.AddCommand(new MoveRightCommand(player));
            //    commandInvoker.ExecuteCommands();
            //}
            //if (Input.GetKeyDown(leftKey)) {
            //    commandInvoker.AddCommand(new MoveLeftCommand(player));
            //    commandInvoker.ExecuteCommands();
            //}

            if (Input.GetKeyDown(undoKey)){
             //commandInvoker.UndoLastCommand();// もう一度同じことをする
            }

            if (Input.GetKeyDown(redoKey)){
                //commandInvoker.RedoLastCommand();// 元に戻したけどやっぱり実行した状態にする
            }
            
        }
    }
}