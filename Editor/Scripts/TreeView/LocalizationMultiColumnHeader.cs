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
#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

namespace CZToolKit.LocalizationText.Editor
{
    public class LocalizationMultiColumnHeader : MultiColumnHeader
    {
        public UnityAction<string> onAddLanguage;
        public UnityAction<string> onRemoveLanguage;
        public UnityAction onLanguagesValueChanged;

        public LocalizationMultiColumnHeader(MultiColumnHeaderState state) : base(state)
        {
            ResizeToFit();
            state.columns[0].autoResize = false;
        }

        protected override void AddColumnHeaderContextMenuItems(GenericMenu menu)
        {
            //base.AddColumnHeaderContextMenuItems(menu);
            menu.AddItem(new GUIContent("ResizeToFit"), false, ResizeToFit);
            menu.AddItem(new GUIContent("Add Item"), false, LocalizationEditorWindow.Current.treeView.AddItem);
            menu.AddItem(new GUIContent("Add Language"), false, AddLanguage);
            for (int i = 1; i < state.columns.Length; i++)
            {
                if (GetColumnRect(i).Contains(Event.current.mousePosition))
                {
                    int tempI = i;
                    menu.AddItem(new GUIContent($"Delete [{GetColumn(tempI).headerContent.text}]"), false, () =>
                    {
                        if (EditorUtility.DisplayDialog("Delete language", string.Concat("Do you want delete [", GetColumn(tempI).headerContent.text, "] language?"), "Yes", "No"))
                        {
                            RemoveLanguage(GetColumn(tempI).headerContent.text);
                            LocalizationEditorWindow.Current.Focus();
                        }
                    });
                    menu.AddItem(new GUIContent($"Rename [{GetColumn(tempI).headerContent.text}]"), false, () =>
                    {
                        string oldName = GetColumn(tempI).headerContent.text;
                        PopupWindow.Show(new Rect(GetColumnRect(tempI).position + Vector2.up * 100, Vector2.zero), new RenameLanguageWindow(oldName, newName => { Rename(oldName, newName); }));
                    });
                    break;
                }
            }
        }

        public override void OnGUI(Rect rect, float xScroll)
        {
            base.OnGUI(rect, xScroll);
            if (Event.current.alt && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
            {
                LocalizationEditorWindow.Current.treeView.AddItem();
                Event.current.Use();
            }
        }

        private void Rename(string oldName, string newName)
        {
            if (newName != oldName)
            {
                if (LocalizationEditorWindow.Current.Languages.ContainsKey(newName))
                {
                    EditorUtility.DisplayDialog("Error", $"The languages is already have [{newName}] language!!", "OK");
                    LocalizationEditorWindow.Current.Focus();
                }
                else
                {
                    LocalizationEditorWindow.Current.RenameLanguage(oldName, newName);
                    LocalizationEditorWindow.Current.treeView.TotalReload();
                }
            }
        }

        private void AddLanguage()
        {
            if (LocalizationEditorWindow.Current.Languages.Count == 32)
            {
                EditorUtility.DisplayDialog("Warning", "最多包含32种语言！！！", "OK");
                LocalizationEditorWindow.Current.Focus();
                return;
            }
            string languageName = "NewLanguage";
            int index = 0;
            while (LocalizationEditorWindow.Current.Languages.Keys.Contains(languageName + index))
            {
                index++;
            }

            languageName = string.Concat(languageName, index);

            LocalizationEditorWindow.Current.AddLanguage(languageName);
            LocalizationEditorWindow.Current.treeView.TotalReload();
        }

        private void RemoveLanguage(string language)
        {
            if (LocalizationEditorWindow.Current.Languages.Count == 1)
            {
                EditorUtility.DisplayDialog("Error", "最少要具有一门语言", "OK");
                LocalizationEditorWindow.Current.Focus();
                return;
            }
            LocalizationEditorWindow.Current.RemoveLanguage(language);
            LocalizationEditorWindow.Current.treeView.TotalReload();
            Debug.Log($"Remove Language:[{language}]");
            if (onLanguagesValueChanged != null)
                onLanguagesValueChanged();
        }

        public static MultiColumnHeader GetHeader()
        {
            MultiColumnHeaderState.Column[] columns = new MultiColumnHeaderState.Column[LocalizationEditorWindow.Current.Languages.Keys.Count + 1];
            int i = 0;
            columns[i] = new MultiColumnHeaderState.Column() { width = 100, minWidth = 50, canSort = true, headerContent = new GUIContent("Key") };
            foreach (string language in LocalizationEditorWindow.Current.Languages.Keys)
            {
                i++;
                columns[i] = new MultiColumnHeaderState.Column() { width = 100, minWidth = 50, canSort = true, headerContent = new GUIContent(language, language) };
            }
            MultiColumnHeaderState multiColumnHeaderState = new MultiColumnHeaderState(columns);
            return new LocalizationMultiColumnHeader(multiColumnHeaderState);
        }
    }
}
#endif