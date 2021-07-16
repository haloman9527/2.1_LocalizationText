#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CZToolKit.LocalizationText.Editor
{
    public class ItemEditorWindow : EditorWindow
    {
        string key = "";
        string[] values;

        GUIStyle textArea;
        private Vector2 mainScroll;
        private Vector2 keyScroll;
        Dictionary<string, Vector2> valueScrolls;

        public UnityAction<string, string[]> onFinished;

        public static ItemEditorWindow OpenEdit(string key)
        {
            ItemEditorWindow window = ScriptableObject.CreateInstance<ItemEditorWindow>();

            Vector2 mouse = GUIUtility.GUIToScreenPoint(new Vector2(100, 100));
            Rect r = new Rect(mouse, Vector2.one);
            window.key = key;

            string[] values;
            if (LocalizationEditorWindow.Current.DataTable.TryGetValue(key, out values))
                window.values = values.Clone() as string[];
            else
                window.values = new string[LocalizationEditorWindow.Current.Languages.Count];

            Vector2 widnowCenter = LocalizationEditorWindow.Current.position.position + LocalizationEditorWindow.Current.position.size / 2;
            window.position = new Rect(widnowCenter - new Vector2(250, 500), new Vector2(500, 400));
            window.titleContent = new GUIContent("Edit Item");
            window.ShowAuxWindow();
            return window;
        }

        public static ItemEditorWindow Open()
        {
            ItemEditorWindow window = ScriptableObject.CreateInstance<ItemEditorWindow>();

            Vector2 mouse = GUIUtility.GUIToScreenPoint(new Vector2(100, 100));
            Rect r = new Rect(mouse, Vector2.one * 10);
            window.values = new string[LocalizationEditorWindow.Current.Languages.Count];

            Vector2 widnowCenter = LocalizationEditorWindow.Current.position.position + LocalizationEditorWindow.Current.position.size / 2;
            window.position = new Rect(widnowCenter - new Vector2(250, 500), new Vector2(500, 400));
            window.titleContent = new GUIContent("Add Item");
            window.ShowAuxWindow();
            return window;
        }

        private void OnEnable()
        {
            valueScrolls = new Dictionary<string, Vector2>();
            foreach (string key in LocalizationEditorWindow.Current.Languages.Keys)
            {
                valueScrolls[key] = new Vector2();
            }
        }

        private void OnDisable()
        {
            EditorWindow.FocusWindowIfItsOpen<LocalizationEditorWindow>();
        }

        private void OnGUI()
        {
            if (textArea == null)
            {
                textArea = new GUIStyle(GUI.skin.textArea);
                textArea.wordWrap = true;
                textArea.stretchHeight = true;
                textArea.fontSize = 15;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Key", GUI.skin.button, GUILayout.Width(100));
            keyScroll = GUILayout.BeginScrollView(keyScroll, GUILayout.Height(textArea.lineHeight * 2));
            key = GUILayout.TextArea(key, textArea);
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            mainScroll = GUILayout.BeginScrollView(mainScroll, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            foreach (var item in LocalizationEditorWindow.Current.Languages)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(item.Key, item.Key), GUI.skin.button, GUILayout.Width(100));
                valueScrolls[item.Key] = GUILayout.BeginScrollView(valueScrolls[item.Key], GUILayout.Height(textArea.lineHeight * 5));
                values[item.Value] = GUILayout.TextArea(values[item.Value], textArea);
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Finished", GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(key))
                    EditorUtility.DisplayDialog("Warning", "key不能为空", "OK");
                else
                    onFinished(key, values);
                Close();
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(30)))
            {
                Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }
}
