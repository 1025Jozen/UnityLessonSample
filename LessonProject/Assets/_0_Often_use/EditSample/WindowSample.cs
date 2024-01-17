using UnityEditor;
using UnityEngine;


namespace CustomEditorSamp
{
    public class WindowSample : EditorWindow
    {
        [MenuItem("Test/ WindowEditor / Sample", false, 1)]//2つめの引数、判定trueで押せなくする。　3つめの引数、メニューアイテムの並び順(数字が小さいほど上)
        private static void ShowWindow() {
            WindowSample window = GetWindow<WindowSample>(); //GetWindow<エディター拡張のクラス>() を呼ぶことで、このエディター拡張のクラスの表示。返り値はこのクラスのオブジェクト
            window.titleContent = new GUIContent("Sample Window");//ウィンドウの名前
        }

        private string text = "";
        private void OnGUI() {

            GUILayout.Label("ラベル表示");
            text = EditorGUILayout.TextArea(text, GUILayout.Height(100));//入力欄  高さを 100px
            GUILayout.Button("ボタン1");//ボタン表示のみ
            if (GUILayout.Button("ボタン2")) {//ボタンを押したら
                Debug.Log(text);
            }








            //ラベルのサイズ設定など
            // いい例。いったんコピーするため他のラベルには影響しない。
            var good = new GUIStyle(EditorStyles.label);
            good.fontSize = 24;
            //EditorGUILayout.LabelField("sample", good);

            //// 悪い例。変数に入れるだけではコピーできない。他のラベルも全てこの設定になってしまう。
            //var bad = EditorStyles.label;
            //bad.fontSize = 24;
            //EditorGUILayout.LabelField("sample", bad);







            //GUILayout.FlexibleSpace() を使うと、GUILayout.FlexibleSpace() が等間隔のスペースになる

            using (new EditorGUILayout.VerticalScope()) {//縦に並ぶ。
                // 上と下にあるから中央揃え
                GUILayout.FlexibleSpace();

                GUILayout.Space(20);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("左揃え", good);
                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope()) {//横に並ぶ。
                    // 下にしかないから左揃え
                    GUILayout.Label("テスト1");
                    GUILayout.Label("テスト2");
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Space(20);

                EditorGUILayout.LabelField("右揃え", good);
                using (new EditorGUILayout.HorizontalScope()) {//横に並ぶ。
                    // 上にしかないから右揃え
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("テスト1");
                    GUILayout.Label("テスト2");
                }

                GUILayout.Space(20);
                EditorGUILayout.LabelField("左右に分かれる", good);
                using (new EditorGUILayout.HorizontalScope()) {//横に並ぶ。
                    // 真ん中にしかないから左右に分かれる
                    GUILayout.Label("テスト1");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("テスト2");
                }

                GUILayout.Space(20);
                EditorGUILayout.LabelField("等間隔", good);
                using (new EditorGUILayout.HorizontalScope()) {//横に並ぶ。
                    // これは等間隔
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("テスト1");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("テスト2");
                    GUILayout.FlexibleSpace();
                }

                GUILayout.Space(20);
                EditorGUILayout.LabelField("やや左による", good);
                using (new EditorGUILayout.HorizontalScope()) {//横に並ぶ。
                    // これはやや左による
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("テスト1");
                    GUILayout.Label("テスト2");
                    GUILayout.FlexibleSpace();
                    GUILayout.FlexibleSpace();
                }

                GUILayout.FlexibleSpace();
            }





        }
    }
}

//GUILayout.Width(float) 要素の幅
//GUILayout.Height(float) 要素の高さ
//GUILayout.MinWidth(float) 要素の最低の幅
//GUILayout.MaxWidth(float) 要素の最大の幅
//GUILayout.MinHeight(float) 要素の最低の高さ
//GUILayout.MaxHeight(float) 要素の最大の高さ
//GUILayout.ExpandWidth(bool) 要素を最大限まで横に伸ばすか
//GUILayout.ExpandHeight(bool) 要素を最大限まで縦に伸ばすか