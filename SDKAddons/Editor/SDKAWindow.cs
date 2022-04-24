#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Net;
using Microsoft.Win32;

namespace SDKAddons
{
    public class SDKAWindow : EditorWindow
    {
        Vector2 scroll;

        public static List<ButtonAPI> ImportButtonContent = new List<ButtonAPI>();

        [MenuItem("VRChat SDK/SDK Addons")]
        static void Init()
        {
            Vector2 minSize = new Vector2(Config.WindowMinSize[0], Config.WindowMinSize[1]);
            SDKAWindow window = (SDKAWindow)GetWindow(typeof(SDKAWindow));
            window.title = Config.WindowTitle;
            window.minSize = minSize;
            window.maxSize = minSize;
            window.Show();
        }

        void OnValidate()
        {
            ImportButtonContent.Clear();
            FetchButtonInfos(Config.AvToolsList, true);
            FetchButtonInfos(Config.WToolsList, false);
            Repaint();
        }

        void OnEnable()
        {
            if (Directory.Exists(Directory.GetCurrentDirectory() + "/Assets/Udon"))
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

            Config.InstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow") + "/AddonsHandler/";
            if (!Directory.Exists(Config.InstallPath)) Directory.CreateDirectory(Config.InstallPath);

            File.WriteAllText(Config.InstallPath + "AvCList.txt", WebTool.ReadTextFromUrl(Config.mainlink + "avc.html"));
            File.WriteAllText(Config.InstallPath + "WCList.txt", WebTool.ReadTextFromUrl(Config.mainlink + "wc.html"));
            Config.AvToolsList = File.ReadAllLines(Config.InstallPath + "AvCList.txt");
            Config.WToolsList = File.ReadAllLines(Config.InstallPath + "WCList.txt");

            Config.Latest = WebTool.ReadTextFromUrl(Config.mainlink);

            ImportButtonContent.Clear();
            FetchButtonInfos(Config.AvToolsList, true);
            FetchButtonInfos(Config.WToolsList, false);

            LoadSettings();
            if (Config.Sel == false && Config.Inf == false) Config.Sel = true;
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
            EditorPrefs.SetInt(Config.CustomEditorPrefs[1], Config.ImpIndex);
            EditorPrefs.SetBool(Config.CustomEditorPrefs[2], Config.Sel);
            EditorPrefs.SetBool(Config.CustomEditorPrefs[3], Config.Inf);
        }

        void LoadSettings()
        {
            Config.ExpIndex = EditorPrefs.GetInt(Config.CustomEditorPrefs[0]);
            Config.ImpIndex = EditorPrefs.GetInt(Config.CustomEditorPrefs[1]);
            Config.Sel = EditorPrefs.GetBool(Config.CustomEditorPrefs[2]);
            Config.Inf = EditorPrefs.GetBool(Config.CustomEditorPrefs[3]);
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
            scroll = GUILayout.BeginScrollView(scroll);
            if(Config.Sel) Selection();
            if(Config.Inf) Info();
            GUILayout.EndScrollView();
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
            Config.ExpIndex = EditorGUILayout.Popup("Editor Mode:", Config.ExpIndex, Config.Experience);
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
            if (Config.IsVRC3Avatar) GenerateButtons("Avatar");
            if (Config.IsVRC3UdonWorld) GenerateButtons("World");
            if (!Config.IsVRC3Avatar && !Config.IsVRC3UdonWorld) GUILayout.Label(
                @"There is no SDK Loaded!
Please change to expert to go on!"
                );
        }

        void ExpertWinow()
        {
            Config.ImpIndex = EditorGUILayout.Popup("Imports:", Config.ImpIndex, Config.Imports);
            switch (Config.ImpIndex)
            {
                case 0:
                    GenerateButtons(Config.Imports[0]);
                    break;
                case 1:
                    GenerateButtons(Config.Imports[1]);
                    break;
                default:
                    break;
            }
        }

        void GenerateButtons(string cat)
        {
            foreach (var button in ImportButtonContent)
            {
                if (button.Category == cat)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(button.Name))
                    {
                        if (!button.Installed)
                        {
                            try
                            {
                                if (!button.Name.Contains("VRCSDK3-WORLD") && !button.Name.Contains("VRCSDK3-AVATAR"))
                                {
                                    using (var client = new WebClient())
                                    {
                                        client.DownloadFile(button.Link + Config.repoDL + button.Name + ".unitypackage", Config.InstallPath + button.Name + ".unitypackage");
                                    }
                                }
                                else
                                {
                                    Application.OpenURL(button.Link + button.Name + ".unitypackage");
                                    string downloads = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
                                    if (File.Exists(downloads + string.Format("/{0}.unitypackage", button.Name)))
                                    {
                                        File.Move(downloads + string.Format("/{0}.unitypackage", button.Name), string.Format("{0}{1}.unitypackage", Config.InstallPath, button.Name));
                                    } 
                                }
                            }
                            catch { }
                            finally
                            {
                                if(File.Exists(string.Format("{0}{1}.unitypackage", Config.InstallPath, button.Name))){
                                    button.Installed = true;
                                    AssetDatabase.ImportPackage(string.Format("{0}{1}.unitypackage", Config.InstallPath, button.Name), true);
                                    AssetDatabase.Refresh();
                                }
                            }
                        }
                        else
                        {
                            AssetDatabase.ImportPackage(string.Format("{0}{1}.unitypackage", Config.InstallPath, button.Name), true);
                            AssetDatabase.Refresh();
                        }
                    }
                    if (GUILayout.Button("Info", GUILayout.Width(80)))
                    {
                        SDKIWindow window = (SDKIWindow)GetWindow(typeof(SDKIWindow));
                        window.title = button.Name;
                        window.Show();
                        SDKIWindow.text = button.Description;
                    }
                    if (button.Installed)
                    {
                        if (GUILayout.Button("Del", GUILayout.Width(50)))
                        {
                            File.Delete(Config.InstallPath + string.Format("{0}.unitypackage", button.Name));
                            button.Installed = false;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }

        void FetchButtonInfos(string[] list, bool Avatar)
        {
            foreach (string item in list)
            {
                string[] text = item.Split('|');
                if (Avatar)
                {
                    if (text[0].Contains("VRCSDK3-AVATAR"))
                    {
                        ImportButtonContent.Add(new ButtonAPI(text[0], text[1], "https://docs.vrchat.com/docs/setting-up-the-sdk", "Avatar", File.Exists(Config.InstallPath + string.Format("{0}.unitypackage", text[0]))));
                    }
                    else
                    {
                        ImportButtonContent.Add(new ButtonAPI(text[0], text[1], WebTool.ReadTextFromUrl(Config.mainlink + "Avatar/" + text[0] + ".html"), "Avatar", File.Exists(Config.InstallPath + string.Format("{0}.unitypackage", text[0]))));
                    }
                }
                else
                {
                    if (text[0].Contains("VRCSDK3-WORLD"))
                    {
                        ImportButtonContent.Add(new ButtonAPI(text[0], text[1], "https://docs.vrchat.com/docs/setting-up-the-sdk", "World", File.Exists(Config.InstallPath + string.Format("{0}.unitypackage", text[0]))));
                    }
                    else
                    {
                        ImportButtonContent.Add(new ButtonAPI(text[0], text[1], WebTool.ReadTextFromUrl(Config.mainlink + "World/" + text[0] + ".html"), "World", File.Exists(Config.InstallPath + string.Format("{0}.unitypackage", text[0]))));
                    }
                }
            }
        }

        void Info()
        {
            EditorGUILayout.LabelField("Is 3.0 World SDK:", YN(Config.IsVRC3UdonWorld));
            EditorGUILayout.LabelField("Is 3.0 Avatar SDK:", YN(Config.IsVRC3Avatar));
            EditorGUILayout.LabelField("Is 2.0 or none SDK:", YN(Config.IsNoneSDK));
            EditorGUILayout.LabelField("Version:", string.Format("{0} - {1}", Config.Current, Config.Latest));
            if(int.Parse(Config.Current) < int.Parse(Config.Latest))
            {
                if (GUILayout.Button("Update"))
                {
                    Application.OpenURL("https://github.com/not90Hz/Addons-Handler/releases/download/Addons-Handler.unitypackage");
                }
            }
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