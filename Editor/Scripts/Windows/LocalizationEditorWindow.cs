using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using CZToolKit.Core.Editors;

namespace CZToolKit.LocalizationText.Editor
{
    public class LocalizationEditorWindow : BasicEditorWindow
    {
        public static LocalizationEditorWindow Current;
        [MenuItem("Tools/CZToolKit/Localization", priority = 4)]
        public static void Open()
        {
            GetWindow<LocalizationEditorWindow>("LocalizationEditor");
        }

        public string filePath;
        public DataFormat dataFormat = DataFormat.Json;
        private Dictionary<string, int> languages = new Dictionary<string, int>();
        private Dictionary<string, string[]> dataTable = new Dictionary<string, string[]>();

        int lineHeight = 16;
        int lineSpace = 5;

        string searchText = "";
        string[] searchTypes;
        int searchFlags = -1;
        SearchField searchField;
        public LanguageTreeView treeView;

        public Dictionary<string, int> Languages { get { return languages; } }
        public Dictionary<string, string[]> DataTable { get { return dataTable; } }

        protected void OnEnable()
        {
            Current = this;

            if (!string.IsNullOrEmpty(filePath))
                Load(filePath, out languages, out dataTable);

            searchField = new SearchField();
            searchField.SetFocus();
            UpdateSearchType();

            treeView = new LanguageTreeView(new TreeViewState(), LocalizationMultiColumnHeader.GetHeader(), searchText, searchFlags);
            treeView.Filtter(searchText, searchFlags);
            treeView.onLanguageCountChanged += UpdateSearchType;
        }

        private void UpdateSearchType()
        {
            searchTypes = new string[this.languages.Count + 1];
            searchTypes[0] = "Key";
            string[] languages = this.languages.Keys.ToArray();
            for (int i = 0; i < languages.Length; i++)
            {
                searchTypes[i + 1] = languages[i];
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(filePath);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", (GUIStyle)"toolbarbutton", GUILayout.Width(100)))
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    Save(filePath);
                else
                {
                    string path = EditorUtility.SaveFilePanelInProject("Save", "language", "json", "", Application.dataPath);
                    if (!string.IsNullOrEmpty(path))
                        Save(path);
                }
            }

            int i = EditorGUILayout.Popup(-1, Enum.GetNames(typeof(DataFormat)), (GUIStyle)"ToolbarDropDownToggle", GUILayout.Width(20));
            if (i != -1)
            {
                string path = "";
                switch (i)
                {
                    case 0:
                        path = EditorUtility.SaveFilePanelInProject("Save", "language", "json", "", Application.dataPath);
                        break;
                    case 1:
                        path = EditorUtility.SaveFilePanelInProject("Save", "language", "csv", "", Application.dataPath);
                        break;
                }
                if (!string.IsNullOrEmpty(path))
                    Save(path);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Load", (GUIStyle)"toolbarbutton", GUILayout.Width(100)))
            {
                string path = EditorUtility.OpenFilePanel("Select", Application.dataPath, "json,csv");
                if (!string.IsNullOrEmpty(path))
                {
                    Load(path, out languages, out dataTable);
                    treeView.TotalReload();
                }
            }

            EditorGUILayout.EndHorizontal();

            Rect rect = new Rect(lineSpace, 30, 50, lineHeight);
            if (GUI.Button(rect, new GUIContent("Reload", "Load from file")) && !string.IsNullOrEmpty(filePath))
            {
                Load(filePath, out languages, out dataTable);
                treeView.TotalReload();
            }

            rect.x += rect.width + lineSpace;
            rect.width = 300;
            searchText = searchField.OnGUI(rect, searchText);
            if (searchField.HasFocus() && Event.current.rawType == EventType.KeyUp && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
            {
                treeView.Filtter(searchText, searchFlags);
                searchField.SetFocus();
            }

            rect.x += 300;
            rect.width = 100;
            searchFlags = EditorGUI.MaskField(rect, searchFlags, searchTypes);

            rect.x += 110;
            rect.width = 20;
            GUI.Button(rect, new GUIContent("?", "Alt + A : 增加条目\nCtrl + C : 复制条目(不支持多选复制)\nCtrl + V : 粘贴条目\nCtrl + D : 粘贴选中条目(不支持多选)\n右键显示菜单"));
            treeView.OnGUI(new Rect(lineSpace, 50, position.width - 10, position.height - 55));
        }

        private void Load(string path, out Dictionary<string, int> languages, out Dictionary<string, string[]> valueTable)
        {
            languages = new Dictionary<string, int>();
            valueTable = new Dictionary<string, string[]>();
            string text = File.ReadAllText(path, Encoding.UTF8);
            dataFormat = path.EndsWith("json") ? DataFormat.Json : DataFormat.CSV;
            LocalizationData data = LocalizationSystem.Load(text, dataFormat);
            languages = data.Languages;
            valueTable = data.DataTable;
            this.filePath = path;
        }

        public void Save(string path)
        {
            dataFormat = path.EndsWith("json") ? DataFormat.Json : DataFormat.CSV;
            switch (dataFormat)
            {
                case DataFormat.Json:
                    File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(ConvertToTable()), Encoding.UTF8);
                    break;
                case DataFormat.CSV:
                    File.WriteAllText(path, CSVLoader.SerializeTable(ConvertToTable()), Encoding.UTF8);
                    break;
                default:
                    break;
            }
            this.filePath = path;
            AssetDatabase.Refresh();
        }

        public void AddLanguage(string languageName)
        {
            languages.Add(languageName, languages.Count);

            string[] keys = dataTable.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string[] newValues = dataTable[keys[i]].Append("").ToArray();
                dataTable[keys[i]] = newValues;
            }
        }

        public void RenameLanguage(string oldName, string newName)
        {
            int index = languages[oldName];
            languages.Remove(oldName);
            languages[newName] = index;
        }

        public void RemoveLanguage(string languageName)
        {
            int index = languages[languageName];

            string[] keys = dataTable.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                List<string> values = dataTable[keys[i]].ToList();
                values.RemoveAt(index);
                dataTable[keys[i]] = values.ToArray();
            }

            languages.Remove(languageName);
            keys = languages.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                languages[keys[i]] = i;
            }
        }

        public void AddItem(string key, string[] textValues)
        {
            for (int i = 0; i < textValues.Length; i++)
            {
                if (textValues[i] == null)
                    textValues[i] = "";
            }

            dataTable[key] = textValues;
        }

        public void EditItem(string oldKey, string newKey, string[] textTextValues)
        {
            RemoveItem(oldKey);
            AddItem(newKey, textTextValues);
        }

        public void RemoveItem(string key)
        {
            dataTable.Remove(key);
        }

        public string[][] ConvertToTable()
        {
            string[][] table = new string[dataTable.Count + 1][];
            table[0] = new string[languages.Count + 1];
            table[0][0] = "Key";
            foreach (KeyValuePair<string, int> item in languages)
            {
                table[0][item.Value + 1] = item.Key;
            }

            int index = 0;
            foreach (KeyValuePair<string, string[]> item in dataTable)
            {
                index++;
                table[index] = new string[languages.Count + 1];
                table[index][0] = item.Key;
                for (int i = 0; i < item.Value.Length; i++)
                {
                    table[index][i + 1] = item.Value[i];
                }
            }
            return table;
        }
    }
}