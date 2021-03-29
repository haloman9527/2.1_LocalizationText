using UnityEditor.IMGUI.Controls;

namespace CZToolKit.LocalizationText.Editor
{
    public class LanguageTreeViewItem : TreeViewItem
    {
        public string[] values;

        public LanguageTreeViewItem()
        {
            this.values = new string[0];
        }

        public LanguageTreeViewItem(string[] _values)
        {
            this.values = _values;
        }

        public LanguageTreeViewItem(int id, string[] _values) : base(id)
        {
            this.values = _values;
        }

        public LanguageTreeViewItem(int id, int depth, string[] _values) : base(id, depth)
        {
            this.values = _values;
        }

        public LanguageTreeViewItem(int id, int depth, string displayName, string[] _values) : base(id, depth, displayName)
        {
            this.values = _values;
        }

        public void Set(int _id, string[] _values)
        {
            id = _id;
            values = _values;
        }

        public void Set(int _id, int _depth, string[] _values)
        {
            Set(_id, _values);
            depth = _depth;
        }

        public void Set(int _id, int _depth, string _displayName, string[] _values)
        {
            Set(_id, _depth, _values);
            displayName = _displayName;
        }
    }
}
