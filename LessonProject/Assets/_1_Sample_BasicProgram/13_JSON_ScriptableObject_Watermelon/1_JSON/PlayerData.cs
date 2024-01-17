using System.Collections.Generic;

namespace JSON
{
    //C#側のスクリプトで記述
    [System.Serializable] //定義したクラスをJSONデータに変換できるようにする
    public class PlayerData
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;


        //辞書型は無視される
        public Dictionary<PlayerMode, PlayerStatus> playerStatus;

        public Item item;

        public List<Item> itemList;
    }

    [System.Serializable]
    public class Item
    {
        public string itemName;
    }

    [System.Serializable] 
    public class PlayerStatus
    {
        public float speed;
    }

    [System.Serializable]
    public enum PlayerMode {
        normal,
        hard
    }
}
