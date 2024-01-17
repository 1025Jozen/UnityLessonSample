using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CustomEditorSamp {
    public class EditorScene : MonoBehaviour
    {

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            //シーンビュー上にテキスト表示
            Handles.Label(transform.position + new Vector3(0, 1, 0), "これはCubeです");
        }
#endif

    }
}

