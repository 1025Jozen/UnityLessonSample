using UnityEngine;
 
namespace UIToolkitExamplesC
{
    [CreateAssetMenu(fileName = "CardDefinition", menuName = "UI_ToolKitSample", order = 0)]
    public class CardDefinition : ScriptableObject 
    {
        //名前　説明　サムネイル　のSO
        public string characterName;
        public string description;
        public Texture2D thumbnail;
    }
}
