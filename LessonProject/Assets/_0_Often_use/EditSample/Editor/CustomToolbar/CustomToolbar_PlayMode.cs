using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomToolbarSample1
{
    //InitializeOnLoadをつけるとUnityEditorが起動したとき、または
    //スクリプトがコンパイルされた直後に静的コンストラクタが自動的に実行されます
    [InitializeOnLoad]
    public static class CustomToolbar_PlayMode
    {
        //静的コンストラクタ　　
        static CustomToolbar_PlayMode() {
            EditorApplication.update += Init;
        }
        private static void Init() {
            EditorApplication.update -= Init;
            InitElements();
        }
        private static void InitElements() {
            var toolbar = UnityEditor.Toolbar.get;
            if (toolbar.windowBackend?.visualTree is not VisualElement visualTree) return;
            if (visualTree.Q("PlayMode") is not { } playButtonZone) return; //ツールバーのPlayModeゾーンを取得


            var testButton = new EditorToolbarButton("CustomButton", () => {
                Debug.LogWarning("CustomButtonが押された");
            });


            playButtonZone.Add(testButton);//playButtonZoneにボタンを追加
        }

    }
}

//下準備
//Editorフォルダ(作成)の下に　CreateからAssembly Definitionを名前は何でもいい(今回はCustomToolbar)　入れる
//Nameを　Unity.InternalAPIEditorBridge.020　に変更




////////////　　　
//visualTree.Q("PlayMode")で要素の取得をしており、
//    別の要素を取得することで他の場所に独自のボタンなどを追加することができます。

//要素名を確認するには、UI Toolkit Debuggerを使用します。
//Window/UI Toolkit/Debuggerから開くことができます。

//Select a Panelの部分からToolbarを選ぶと左側に要素がTree上に表示されます。

//先ほど作成したEditorToolbrButton(図の下から二番目の要素)は"PlayMode"の要素を取得して追加したため、取得した要素の子要素として追加されています。