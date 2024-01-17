using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitExamplesB
{
    public class Card : VisualElement
    {
        //キャラ名　説明　サムネイル
        private Label m_characterName;
        private Label m_description;
        private VisualElement m_thumbnail;

        //どこにも使っていないように見えるが、UI Builder上で値を保持するために使用される
        private string CharacterName => m_characterName.text;
        private string Description   => m_description.text;




        public new class UxmlFactory : UxmlFactory<Card, UxmlTraits> { }




        public new class UxmlTraits : VisualElement.UxmlTraits　//継承
        {
            //UI Buikderから値を受け取っている
            public UxmlStringAttributeDescription _characterName       = new UxmlStringAttributeDescription { name = "character-name", defaultValue = "Name" };
            public UxmlStringAttributeDescription _description         = new UxmlStringAttributeDescription { name = "description", defaultValue = "Description" };
            public UxmlAssetAttributeDescription<Texture2D> _thumbnail = new() { name = "thumbnail"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var card = ve as Card;
                card.m_characterName.text = _characterName.GetValueFromBag(bag, cc);
                card.m_description.text = _description.GetValueFromBag(bag, cc);
                card.m_thumbnail.style.backgroundImage = _thumbnail.GetValueFromBag(bag, cc);
            }
        }



        //コンストラクタ
        public Card()
        {
            var treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Sample/Etc/06_UI_ToolKit/Card.uxml");
            var container = treeAsset.Instantiate();
            hierarchy.Add(container);



            //UI　Builderで配置(用意)した　名前・説明・サムネイル　に代入
            m_characterName = container.Q<Label>("Name");
            m_description   = container.Q<Label>("Description");
            m_thumbnail     = container.Q<VisualElement>("Thumbnail");




            var card = container.Q<VisualElement>("Root");
            card.AddManipulator(new Clickable(()=> Debug.Log("Click")));
        }
    }
}
