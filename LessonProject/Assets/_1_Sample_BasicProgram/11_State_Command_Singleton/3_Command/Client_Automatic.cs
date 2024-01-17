using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Command
{
    public class Client_Automatic : MonoBehaviour
    {
        CommandInvoker commandInvoker;//クラス内にコマンドを保持するstack　< IComand > がある
        [SerializeField] PlayerMove player;

        ICommand upCommand;
        ICommand downCommand;
        ICommand rightCommand;
        ICommand leftCommand;


        private float repeatSpan = 3;    //繰り返す間隔
        private float timeElapsed = 0;   //経過時間

        ICommand command;//実行するコマンド
        int commandNumber;

        private void Awake()
        {
            //player         = new PlayerMove();     //プレイヤーの動き　インスタンス化
            commandInvoker = new CommandInvoker(); //

            upCommand    = new MoveUpCommand(player);
            downCommand  = new MoveDownCommand(player); 
            rightCommand = new MoveRightCommand(player);
            leftCommand  = new MoveLeftCommand(player);
        }


        void Start() {
            // プレイヤーの移動コマンドを追加　commandInvoker　内の　commandListに　ICommandを継承したクラス　を　追加
            commandInvoker.AddCommand(upCommand);   //リストにインスタンスを保存　　IComandを継承したクラス　コマンドごとに挙動を分けている
            commandInvoker.AddCommand(downCommand);
            commandInvoker.AddCommand(rightCommand);
            commandInvoker.AddCommand(leftCommand);
        }
        void Update() {
            //commandInvoker.ExecuteCommands();// コマンドの実行   リストの中のインスタンス.Execute()を順番に実行している  

            timeElapsed += Time.deltaTime;     //時間をカウントする           
            if (timeElapsed >= repeatSpan) {//経過時間が繰り返す間隔を経過したら
                if (commandNumber >= commandInvoker.commandList.Count) {
                    command = null;
                    commandNumber = commandInvoker.commandList.Count;
                } else {
                    command = commandInvoker.commandList[commandNumber];
                }               
                commandNumber += 1;//次のリストに
                timeElapsed = 0;   //経過時間をリセットする
            }
            if (command != null) {
                command.Execute();//リスト内のコマンドを実行
            }
             

        }
    }
}