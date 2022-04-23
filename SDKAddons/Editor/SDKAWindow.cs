#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SDKAddons
{
    public class SDKAWindow : EditorWindow
    {

        [MenuItem("VRChat SDK/Tools/SDK Addons")]
        static void Init()
        {
            Vector2 minSize = new Vector2(Config.WindowMinSize[0], Config.WindowMinSize[1]);
            SDKAWindow window = (SDKAWindow)GetWindow(typeof(SDKAWindow));
            window.title = Config.WindowTitle;
            window.minSize = minSize;
            window.maxSize = minSize;
            window.Show();
        }

        void OnEnable()
        {
            if(Directory.Exists(Directory.GetCurrentDirectory() + "/Assets/Udon"))
            {
                Config.IsVRC3UdonWorld = true;
                Config.IsVRC3Avatar = false;
                Config.IsNoneSDK = false;
            }
            if(Directory.Exists(Directory.GetCurrentDirectory() + "/Assets/VRCSDK/SDK3A"))
            {
                Config.IsVRC3UdonWorld = false;
                Config.IsVRC3Avatar = true;
                Config.IsNoneSDK = false;

            }
            if(Config.IsVRC3UdonWorld == false && Config.IsVRC3Avatar == false)
            {
                Config.IsVRC3UdonWorld = false;
                Config.IsVRC3Avatar = false;
                Config.IsNoneSDK = true;
            }

            


            if (Config.Sel == false && Config.Inf == false) Config.Sel = true;
            LoadSettings();
        }

        void OnDestroy()
        {
            SaveSettings();
        }

        void OnDisable()
        {
            SaveSettings();
        }

        void SaveSettings()
        {
            EditorPrefs.SetInt(Config.CustomEditorPrefs[0], Config.ExpIndex);
        }

        void LoadSettings()
        {
            Config.ExpIndex = EditorPrefs.GetInt(Config.CustomEditorPrefs[0]);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void OnGUI()
        {
            NewWindow();
        }

        void NewWindow()
        {
            GUILayout.BeginVertical();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            GUILayout.BeginVertical(GUI.skin.window);
            GUILayout.BeginHorizontal();
            SelInf();
            GUILayout.EndHorizontal();
            if(Config.Sel) Selection();
            if(Config.Inf) Info();
            GUILayout.EndVertical();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            GUILayout.EndVertical();

            SaveSettings();
        }

        void SelInf()
        {
            if (GUILayout.Button("Tools"))
            {
                Config.Sel = true;
                Config.Inf = false;
            }
            if (GUILayout.Button("Info"))
            {
                Config.Sel = false;
                Config.Inf = true;
            }
        }

        void Selection()
        {
            Config.ExpIndex = EditorGUILayout.Popup("Your Experience Mode:", Config.ExpIndex, Config.Experience);
            GUILayout.BeginVertical(GUI.skin.box);
            switch (Config.ExpIndex)
            {
                case 0:
                    NewUserWindow();
                    break;
                case 1:
                    ExpertWinow();
                    break;
                default:
                    break;
            }
            GUILayout.EndVertical();
        }

        void NewUserWindow()
        {
            
        }

        void ExpertWinow()
        {
            GUILayout.Label("Bye");
        }

        void Info()
        {
            EditorGUILayout.LabelField("Is 3.0 World SDK:", YN(Config.IsVRC3UdonWorld));
            EditorGUILayout.LabelField("Is 3.0 Avatar SDK:", YN(Config.IsVRC3Avatar));
            EditorGUILayout.LabelField("Is 2.0 or none SDK:", YN(Config.IsNoneSDK));
        }

        public static string YN(bool yn)
        {
            string IsYN;
            if (yn)
            {
                IsYN = "Yes";
            }
            else
            {
                IsYN = "No";
            }
            return IsYN;
        }
    }
}
#endif