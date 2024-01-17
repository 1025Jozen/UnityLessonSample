using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamplesA
{
    public class Card : VisualElement//継承
    {
        public new class UxmlFactory : UxmlFactory<Card> { }//継承(UI Builder上から呼び出す際に必要なもの)

        public Card()//コンストラクタ
        {
            var treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Sample/Etc/06_UI_ToolKit/Card.uxml");//コンストラクタ内で呼び出したいuxmlファイルのロード
            var container = treeAsset.Instantiate();//生成して配置
            hierarchy.Add(container);

            var card = container.Q<VisualElement>("Root");//Cardの背景の名前 "Root"   container.Qで最初のVisualElement"Root"要素を返す
            card.AddManipulator(new Clickable(() => Debug.Log("Click")));//card要素にクリックを追加　押したらDebug表示
        }
    }
}
