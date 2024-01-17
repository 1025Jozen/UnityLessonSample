using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamplesC
{
    public class Card : VisualElement
    {
        private Label m_characterName;
        private Label m_description;
        private VisualElement m_thumbnail;

        public new class UxmlFactory : UxmlFactory<Card, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //SOで用意した型を取得
            public UxmlAssetAttributeDescription<CardDefinition> _card = new() { name = "card-definition" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                //_cardについての情報(名前　説明　サムネイル)を代入
                base.Init(ve, bag, cc);
                if (_card.TryGetValueFromBag(bag, cc, out CardDefinition value))
                {
                    var card = ve as Card;
                    card.m_characterName.text = value.characterName;
                    card.m_description.text = value.description;
                    card.m_thumbnail.style.backgroundImage = value.thumbnail;
                }
            }
        }

        //コンストラクタ
        public Card()
        {
            var treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Sample/Etc/06_UI_ToolKit/Card.uxml");
            var container = treeAsset.Instantiate();
            hierarchy.Add(container);

            m_characterName = container.Q<Label>("Name");
            m_description = container.Q<Label>("Description");
            m_thumbnail = container.Q<VisualElement>("Thumbnail");

            var card = container.Q<VisualElement>("Root");
            card.AddManipulator(new Clickable(()=>Debug.Log("Click")));
        }
    }
}

