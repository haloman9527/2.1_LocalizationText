using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Events;
using System;

namespace CZToolKit.LocalizationText.Editor
{
    public class LanguageTreeView : TreeView
    {
        public UnityAction onLanguageCountChanged;

        private int searchFlags;
        private TreeViewItem copyCache;
        private LocalizationMultiColumnHeader header;
        private Dictionary<int, TreeViewItem> idItems = new Dictionary<int, TreeViewItem>();
        private Dictionary<string, TreeViewItem> keyItems = new Dictionary<string, TreeViewItem>();

        private LanguageTreeViewItemPool treeViewItemPool = new LanguageTreeViewItemPool();

        public LanguageTreeView(TreeViewState state) : base(state)
        {
            rowHeight = 20;
            showBorder = true;
            this.searchFlags = -1;
            Reload();
        }

        public LanguageTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, string searchText, int searchFlags) : base(state, multiColumnHeader)
        {
            rowHeight = 20;
            showBorder = true;
            header = multiColumnHeader as LocalizationMultiColumnHeader;
            this.searchString = searchText;
            this.searchFlags = searchFlags;
            Reload();
        }

        public void Filtter(string searchString, int searchType)
        {
            this.searchString = searchString;
            this.searchFlags = searchType;
            Reload();
        }

        public void TotalReload()
        {
            if (onLanguageCountChanged != null)
                onLanguageCountChanged();
            this.multiColumnHeader = LocalizationMultiColumnHeader.GetHeader();
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };

            root.children = new List<TreeViewItem>();
            treeViewItemPool.RecycleAll();
            
            idItems.Clear();
            keyItems.Clear();
            char[] search = Convert.ToString(searchFlags, 2).PadLeft(LocalizationEditorWindow.Current.Languages.Count + 1, '0').ToCharArray();
            int id = -1;

            foreach (KeyValuePair<string, string[]> pair in LocalizationEditorWindow.Current.DataTable)
            {
                if (searchFlags == 0)
                    continue;
                //首先匹配Key，若匹配到，则显示此条数据并继续下一条
                if (string.IsNullOrEmpty(searchString) || (searchFlags == -1 || search[search.Length - 1] == '1') && pair.Key.ToLower().Contains(searchString.ToLower()))
                {
                    id++;
                    LanguageTreeViewItem item = treeViewItemPool.Spawn();
                    //LanguageTreeViewItem item = new LanguageTreeViewItem(id, 0, pair.Key, pair.Value);
                    item.Set(id, 0, pair.Key, pair.Value);
                    idItems[id] = item;
                    keyItems[pair.Key] = item;
                    root.children.Add(item);
                    continue;
                }
                for (int i = 0; i < pair.Value.Length; i++)
                {
                    int index = i;
                    if ((searchFlags == -1 || search[search.Length - index - 2] == '1') && pair.Value[index].ToLower().Contains(searchString.ToLower()))
                    {
                        id++;
                        LanguageTreeViewItem item = new LanguageTreeViewItem(id, 0, pair.Key, pair.Value);
                        idItems[id] = item;
                        keyItems[pair.Key] = item;
                        root.children.Add(item);
                        break;
                    }
                }
            }

            root.children.Sort((left, right) =>
            {
                for (int i = 0; i < left.displayName.Length; i++)
                {
                    if (i >= right.displayName.Length || left.displayName[i] > right.displayName[i])
                        return 1;
                    if (left.displayName[i] < right.displayName[i])
                        return -1;
                }

                return 0;
            });
            //foreach (var item in root.children)
            //{
            //    Debug.Log((item as LanguageTreeViewItem).values[0]);
            //}
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void BeforeRowsGUI()
        {
            base.BeforeRowsGUI();

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
            {
                RemoveSelectionItems();
            }
            if (Event.current.control && Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.C)
                {
                    List<int> selectedNodes = GetSelection().ToList();
                    if (selectedNodes.Count == 1)
                        Copy(idItems[selectedNodes[0]]);
                }
                else if (Event.current.keyCode == KeyCode.D)
                {
                    List<int> selectedNodes = GetSelection().ToList();
                    if (selectedNodes.Count == 1)
                        Paste(idItems[selectedNodes[0]]);
                }
                else if (Event.current.keyCode == KeyCode.V)
                {
                    if (copyCache != null)
                        Paste(copyCache);
                }
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            LanguageTreeViewItem item = args.item as LanguageTreeViewItem;
            Rect rect = args.GetCellRect(0);
            if (multiColumnHeader.IsColumnVisible(0))
            {
                GUI.Label(rect, new GUIContent(item.displayName, item.displayName));
                GUI.Box(new Rect(rect.x + rect.width, rect.y, 2, rect.height), "");
            }

            //int showIndex = 1;
            for (int i = 0; i < item.values.Length; i++)
            {
                if (multiColumnHeader.IsColumnVisible(i + 1))
                {
                    rect = args.GetCellRect(i+1);
                    GUI.Label(rect, new GUIContent(item.values[i], item.values[i]));
                    GUI.Box(new Rect(rect.x + rect.width, rect.y, 2, rect.height), "");
                    //showIndex++;
                }
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);
            EditItem(id);
        }

        protected override void ContextClickedItem(int id)
        {
            List<int> selections = GetSelection().ToList();
            if (selections.Count > 0)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add"), false, AddItem);
                menu.AddItem(new GUIContent("Remove"), false, () => { RemoveSelectionItems(selections); });
                if (selections.Count == 1)
                {
                    menu.AddItem(new GUIContent("Edit"), false, EditItem, selections[0]);
                    menu.AddItem(new GUIContent("Copy"), false, Copy, selections[0]);
                    if (copyCache != null)
                        menu.AddItem(new GUIContent("Paste"), false, Paste, copyCache);
                }
                menu.ShowAsContext();
            }
        }

        private void Copy(object obj)
        {
            copyCache = idItems[(int)obj];
        }

        private void Paste(object obj)
        {
            TreeViewItem item = obj as TreeViewItem;
            ItemEditorWindow itemEditorWindow = ItemEditorWindow.OpenEdit(item.displayName);
            itemEditorWindow.titleContent = new GUIContent("Add Item");
            itemEditorWindow.onFinished = (key, values) =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    EditorUtility.DisplayDialog("Warning", "key不能为空", "OK");
                    LocalizationEditorWindow.Current.Focus();
                    return;
                }

                if (!LocalizationEditorWindow.Current.DataTable.ContainsKey(key))
                {
                    LocalizationEditorWindow.Current.AddItem(key, values);
                    LocalizationEditorWindow.Current.treeView.Reload();
                }
                else if (EditorUtility.DisplayDialog("Warning", $"已经包含名为[{key}]的Key，是否替换？", "Yes", "No"))
                {
                    LocalizationEditorWindow.Current.AddItem(key, values);
                    LocalizationEditorWindow.Current.treeView.Reload();
                    LocalizationEditorWindow.Current.Focus();
                }
            };
        }

        public void AddItem()
        {
            ItemEditorWindow itemEditorWindow = ItemEditorWindow.Open();
            itemEditorWindow.titleContent = new GUIContent("Add Item");
            itemEditorWindow.onFinished = (key, values) =>
            {
                if (string.IsNullOrEmpty(key))
                {
                    EditorUtility.DisplayDialog("Warning", "key不能为空", "OK");
                    LocalizationEditorWindow.Current.Focus();
                    return;
                }

                if (!LocalizationEditorWindow.Current.DataTable.ContainsKey(key))
                {
                    LocalizationEditorWindow.Current.AddItem(key, values);
                    Reload();
                }
                else if (EditorUtility.DisplayDialog("Warning", $"已经包含名为[{key}]的Key，是否替换？", "Yes", "No"))
                {
                    LocalizationEditorWindow.Current.AddItem(key, values);
                    LanguageTreeViewItem targetItem = keyItems[key] as LanguageTreeViewItem;
                    targetItem.values = values;
                    LocalizationEditorWindow.Current.Focus();
                }
            };
        }

        private void EditItem(object obj)
        {
            TreeViewItem item = idItems[(int)obj];
            ItemEditorWindow itemEditorWindow = ItemEditorWindow.OpenEdit(item.displayName);

            itemEditorWindow.titleContent = new GUIContent("Edit Item");
            string oldKey = item.displayName;
            itemEditorWindow.onFinished = (newKey, values) =>
            {
                if (newKey == oldKey || !LocalizationEditorWindow.Current.DataTable.ContainsKey(newKey))
                {
                    itemEditorWindow.Close();
                    LocalizationEditorWindow.Current.EditItem(oldKey, newKey, values);
                    item.displayName = newKey;
                    (item as LanguageTreeViewItem).values = values;
                }
                else if (EditorUtility.DisplayDialog("Warning", $"已经包含名为[{newKey}]的Key，是否替换？", "Yes", "No"))
                {
                    itemEditorWindow.Close();
                    LocalizationEditorWindow.Current.EditItem(oldKey, newKey, values);
                    Reload();
                    LocalizationEditorWindow.Current.Focus();
                }
            };
        }

        private void RemoveSelectionItems(List<int> selections = null)
        {
            if (selections == null)
                selections = GetSelection().ToList();

            if (selections.Count > 0 && EditorUtility.DisplayDialog("Delete item(s)", "Do you want delete these item(s)?", "Yes", "No"))
            {
                int totalCount = selections.Count;
                int currentCount = 0;
                EditorApplication.update += () =>
                {
                    bool cancel = EditorUtility.DisplayCancelableProgressBar("Hold On", "", (float)currentCount / totalCount);
                    if (cancel || currentCount == totalCount)
                    {
                        Reload();
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        return;
                    }

                    int frameCurrentCount = selections.Count >= 10000 ? 10000 : selections.Count;
                    int frameTotalCount = frameCurrentCount;

                    foreach (int id in selections)
                    {
                        LocalizationEditorWindow.Current.RemoveItem(idItems[id].displayName);
                        if (--frameCurrentCount == 0)
                            break;
                    }

                    currentCount += frameTotalCount;
                    selections.RemoveRange(0, frameTotalCount);
                };
            }
        }
    }
}