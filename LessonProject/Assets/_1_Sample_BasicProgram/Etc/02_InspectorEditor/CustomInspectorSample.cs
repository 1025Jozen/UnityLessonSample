//******注意
//Unityのエディター拡張で設定した値は通常、
//ゲームが再生モードになると破棄されます。
//設定を保存するためにカスタムのScriptableObjectを作成し、
//エディターとゲームで共有できるようにします。

//(エディター内だけでエディター拡張で設定した値を使用するのであれば
//EditorPrefsの利用やクラスの[System.Serializable]化などの方法はある

using UnityEngine;
using NUnit.Framework.Interfaces;
#if UNITY_EDITOR//もしくは Editorフォルダを作成しそこに入れる
using UnityEditor;
#endif


namespace CustomEditorSamp
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SampleRoot))]
    public class CustomInspectorSample : Editor
    {
        public override void OnInspectorGUI() {
            SampleRoot textData = (SampleRoot)target;

            // テキストデータを編集
            EditorGUI.BeginChangeCheck();
            string labelText = EditorGUILayout.TextField("ラベルテキスト", textData.labelText);
            if (EditorGUI.EndChangeCheck()) {                
                textData.labelText = labelText;// ユーザーがテキストを変更したら保存
                EditorUtility.SetDirty(target);
            }          
            EditorGUILayout.LabelField("保存した値", textData.labelText);// 保存したテキストを表示

            //ラベルを変えたいだけなら　SOにしなくてもいいが値を保存したいならSOを使う必要がある(プログラム作成時点では)
            serializedObject.Update();//変数を最新の状態にして
          
            //using (new EditorGUI.DisabledGroupScope(true))//クリックした時ファイルへ飛べるように
            //{
            //    EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            //}

            //このメソッドでインスペクターの表示を変更できる
            serializedObject.FindProperty(nameof(SampleRoot.enemyName)).stringValue = EditorGUILayout.TextField("敵の名前", serializedObject.FindProperty(nameof(SampleRoot.enemyName)).stringValue);
            serializedObject.FindProperty(nameof(SampleRoot.hp)).intValue = EditorGUILayout.IntField("体力", serializedObject.FindProperty(nameof(SampleRoot.hp)).intValue);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SampleRoot.power)));

            serializedObject.ApplyModifiedProperties();//変数の変更を保存 //これらがないと変数が保存されない

        }
        /*
        //public static Vector3 vec3;
        ////-------------------------------------------------------
        ////0001 0010 0100
        //public enum Animal { dog = 1 << 0, cat = 1 << 1, bear = 1 << 2 }//  1  10  100   ビット演算　0番目に1  1番目に1  
        //public static Animal animalKind;
        //-------------------------------------------------------
        bool[] sample1 = new bool[2] { true, true };
        bool[] sample2 = new bool[2] { true, true };

        bool sample1Enabled = true;
        bool sample2Enabled = false;
        //-------------------------------------------------------
        bool isOpen;






        //このメソッドでインスペクターの表示を変更できる
        public override void OnInspectorGUI() {

            serializedObject.Update();//変数を最新の状態にして



            using (new EditorGUI.DisabledGroupScope(true))//クリックした時ファイルへ飛べるように
            {
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
            }

            //このメソッドでインスペクターの表示を変更できる
            serializedObject.FindProperty(nameof(SampleRoot.enemyName)).stringValue = EditorGUILayout.TextField("敵の名前", serializedObject.FindProperty(nameof(SampleRoot.enemyName)).stringValue);
            serializedObject.FindProperty(nameof(SampleRoot.hp)).intValue = EditorGUILayout.IntField("体力", serializedObject.FindProperty(nameof(SampleRoot.hp)).intValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(SampleRoot.power)));




            GUILayout.Space(20); //EditorGUILayout.Space();
            

            //-------------------------------------------------------
            GUILayout.Space(20);
            SampleRoot.animalKind = (SampleRoot.Animal)EditorGUILayout.EnumFlagsField("Animal", SampleRoot.animalKind);
            SampleRoot.vec3 = EditorGUILayout.Vector3Field("Sample", SampleRoot.vec3);

            if (GUILayout.Button("ぼたんだよ！")) {//クリックされると true を返す
                Debug.Log("押されたよ！");
                Debug.Log(SampleRoot.animalKind);//全て選択は -1  001 1     011  3     110   6   101  5
                Debug.Log(SampleRoot.vec3);
            }            
            ////---------------------------------------------------------------------------------------------
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Sample");
            EditorGUILayout.LabelField("とっても、とーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーても長い文章", EditorStyles.wordWrappedLabel);
            EditorGUILayout.LabelField("太いラベルだよ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("小さいラベルだよ", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("大きいラベルだよ", EditorStyles.largeLabel);
            EditorGUILayout.LabelField("白いラベルだよ", EditorStyles.whiteLabel);
            ////---------------------------------------------------------------------------------------------
            GUILayout.Space(20);
            GUILayout.Label("　NuGet importer for Unity のページに飛ぶ", EditorStyles.linkLabel);
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            Event nowEvent = Event.current;
            if (nowEvent.type == EventType.MouseDown && rect.Contains(nowEvent.mousePosition)) {
                Help.BrowseURL(@"https://kumas-nu.github.io/NuGet-importer-for-Unity/#%E6%97%A5%E6%9C%AC%E8%AA%9E");
            }
            ////---------------------------------------------------------------------------------------------
            GUILayout.Space(20);

            EditorGUILayout.LabelField(new GUIContent("Sample", "tooltip"));
            //---------------------------------------------------------------------------------------------
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("情報だよ！わーい！", MessageType.Info);
            
            //-------------------------------------------------------
            GUILayout.Space(20);
            //グループ内の物を有効化させたり無効化させたりする。
            using (var sample1Group = new EditorGUILayout.ToggleGroupScope("Sample1", sample1Enabled)) {
                sample1Enabled = sample1Group.enabled;
                sample1[0] = EditorGUILayout.Toggle("Sample1-1", sample1[0]);
                sample1[1] = EditorGUILayout.Toggle("Sample1-2", sample1[1]);
            }

            using (var sample2Group = new EditorGUILayout.ToggleGroupScope("sample2", sample2Enabled)) {
                sample2Enabled = sample2Group.enabled;
                sample2[0] = EditorGUILayout.Toggle("Sample2-1", sample2[0]);
                sample2[1] = EditorGUILayout.Toggle("Sample2-2", sample2[1]);
            }
            //--------------------------------------------------------------------
            GUILayout.Space(20);
            isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(isOpen, "Sample");
            if (isOpen) {
                EditorGUILayout.LabelField("開いてるよ");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            //-------------------------------------------------------
            GUILayout.Space(20);
            //要素そのものの色を変える
            var color1 = GUI.color;
            GUI.color = Color.yellow;
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.ExpandWidth(true))) {
                GUI.color = color1;
                EditorGUILayout.LabelField("きいろくなった！！");
            }
            //-------------------------------------------------------
            GUILayout.Space(20);
            //要素の背景の色を変える。
            var color2 = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.ExpandWidth(true))) {
                GUI.backgroundColor = color2;
                EditorGUILayout.LabelField("きいろくなった！！");
            }
            //-------------------------------------------------------
            GUILayout.Space(20);
            //仕切りを書く。
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("前だよ");
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("後ろだよ");
            //-------------------------------------------------------




            serializedObject.ApplyModifiedProperties();//変数の変更を保存 //これらがないと変数が保存されない
        }
        */
    }
#endif
}