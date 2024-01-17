namespace Command
{
    //今回は同じファイルに書いたが分けていい

    //クラスとして動き方を持つ　



    public class MoveUpCommand : ICommand
    {
        private PlayerMove player;

        public MoveUpCommand(PlayerMove player) {
            this.player = player;
        }

        //実際に実行
        public void Execute() {
            player.MoveUp();
        }
    }

    public class MoveDownCommand : ICommand
    {
        private PlayerMove player;

        public MoveDownCommand(PlayerMove player) {
            this.player = player;
        }

        //実際に実行
        public void Execute() {
            player.MoveDown();
        }
    }

    public class MoveLeftCommand : ICommand
    {
        private PlayerMove player;

        public MoveLeftCommand(PlayerMove player) {
            this.player = player;
        }

        //実際に実行
        public void Execute() {
            player.MoveLeft();
        }
    }

    public class MoveRightCommand : ICommand
    {
        private PlayerMove player;

        public MoveRightCommand(PlayerMove player) {
            this.player = player;
        }

        //実際に実行
        public void Execute() {
            player.MoveRight();
        }
    }
}
