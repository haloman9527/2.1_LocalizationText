using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CZToolKit.LocalizationText.Editor
{
    public class RenameLanguageWindow : EditorWindow
    {
        public static RenameLanguageWindow Open(Vector2 position)
        {
            RenameLanguageWindow window = ScriptableObject.CreateInstance<RenameLanguageWindow>();
            window.ShowAsDropDown(new Rect(position, Vector2.one * 10), new Vector2(200, 50));
            return window;
        }

        public string languageName;
        public UnityAction<string> onFinished;

        private void OnGUI()
        {
            languageName = GUILayout.TextField(languageName);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Finished"))
            {
                onFinished(languageName);
                Close();
            }
            GUILayout.EndHorizontal();
        }

        private void OnDisable()
        {
            EditorWindow.FocusWindowIfItsOpen<LocalizationEditorWindow>();
        }
    }
}
