using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomToolbarSample1
{
    public static class CustomToolbar_ToolbarZoneLeftAlign
    {
        [InitializeOnLoadMethod]//[InitializeOnLoad]をつけずにこれでも実行される
        private static void InitializeOnLoad() {
            EditorApplication.update += OnUpdate;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingPlayMode) {
                // プレイモードを終了する際に実行する処理
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(null);
            }
        }
    

        private static void OnUpdate() {
            var toolbar = UnityEditor.Toolbar.get;                                         // ツールバーの取得
            if (toolbar.windowBackend?.visualTree is not VisualElement visualTree) return; // ツールバーのVisualTreeを取得 visualTreeに保存
            if (visualTree.Q("ToolbarZoneLeftAlign") is not { } leftZone) return;          // ツールバー左側のゾーンを取得   leftZoneに保存
            EditorApplication.update -= OnUpdate;                                          // 描画は一回のみでよい

            // VisualElementの追加　　ToolbarButton要素を作成
            var sampleButton = new ToolbarButton() {
                style = { width = 350 }                  // 画像によってボタンが大きくなりすぎてしまわないように幅を制限
            };

            sampleButton.clicked += () => Debug.Log("Sample Buttonがクリックされました");//ボタンの挙動を設定していく
            sampleButton.clicked += () => PlayScene();


            //ラベル表示などの見た目を設定
            sampleButton.Add(new Label("チュートリアルシーン再生") {
                // 文字の見た目などの調整
                style = {
                    fontSize = 10,
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold)
                }
            });

            //イメージ画像も付与
            sampleButton.Add(new Image() {
                image = EditorGUIUtility.IconContent("d_DebuggerAttached@2x").image // デバッグっぽいアイコン
            });


            leftZone.Add(sampleButton);//該当箇所にボタン要素を追加



        }

    //特定シーンの再生処理
    static void PlayScene() {
            string scenePath = EditorBuildSettings.scenes[2].path;//シーンの設定
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            if (sceneAsset == null) {
                Debug.Log($"{scenePath} シーンアセットが存在しません");
                return;
            }

            EditorSceneManager.playModeStartScene = sceneAsset;

            //EditorApplication.isPlaying = true;


            if (!EditorApplication.isPlaying)//Playモードじゃなかったら
            {
                EditorApplication.isPlaying = true;
            } else {
                
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(null);
                EditorApplication.isPlaying = false;
            }
        }
    }





}

//下準備
//Editorフォルダ(作成)の下に　CreateからAssembly Definitionを名前は何でもいい(今回はCustomToolbar)　入れる
//Nameを　Unity.InternalAPIEditorBridge.020　に変更




////////////　　　
//visualTree.Q("ToolbarZoneLeftAlign")で要素の取得をしており、
//    別の要素を取得することで他の場所に独自のボタンなどを追加することができます。

//要素名を確認するには、UI Toolkit Debuggerを使用します。
//Window/UI Toolkit/Debuggerから開くことができます。

//Select a Panelの部分からToolbarを選ぶと左側に要素がTree上に表示されます。

//先ほど作成したEditorToolbrButton(図の下から二番目の要素)は"PlayMode"の要素を取得して追加したため、取得した要素の子要素として追加されています。




//    if (!EditorApplication.isPlaying)//Playモードじゃなかったら
//    {
//        //PlayScene();//実行
//        Debug.Log("ボタン");
//    }
//    else
//    {
//        Debug.Log("ボタン");
//        EditorApplication.isPlaying = false;
//    }