using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomEditorSample
{

    public class OpenURL : EditorWindow
    {
        [MenuItem("OpenURL/ML-agents")]
        static void Open() {
            GetWindow<OpenURL>("OpenURL");
        }

        void OnGUI() {
            EditorGUILayout.LabelField($"現在のサイズ : {position.size}");

            minSize = new Vector2(200f, 50f);
            maxSize = new Vector2(300f, 100f);

            //using var scope = new EditorGUILayout.VerticalScope();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Open Documentation ML-Agents")) {
                //任意のURLを開く
                Application.OpenURL("https://drive.google.com/drive/folders/1Qxd4PeikBb7pztRt8RDlirOxi1tCofYk?usp=drive_link");
            }

            GUILayout.FlexibleSpace();

        }
    }

}