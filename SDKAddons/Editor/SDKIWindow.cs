using UnityEditor;
using UnityEngine;

namespace SDKAddons
{
    public class SDKIWindow : EditorWindow
    {
        public static string text = "";

        Vector2 scroll;

        void OnGUI()
        {
            scroll = GUILayout.BeginScrollView(scroll);
            GUILayout.BeginVertical();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            GUILayout.BeginVertical(GUI.skin.window);
            GUILayout.Label(text);
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }
}
