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
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CZToolKit.LocalizationText.Editor
{
    public class RenameLanguageWindow : PopupWindowContent
    {
        public string languageName;
        public UnityAction<string> onFinished;

        public RenameLanguageWindow(string _oldName, UnityAction<string> _onFinished)
        {
            languageName = _oldName;
            onFinished = _onFinished;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 50);
        }

        public override void OnGUI(Rect rect)
        {
            languageName = GUILayout.TextField(languageName);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Finished"))
            {
                onFinished(languageName);
                editorWindow.Close();
            }
            GUILayout.EndHorizontal();
        }

        public override void OnClose()
        {
            base.OnClose();
            EditorWindow.FocusWindowIfItsOpen<LocalizationEditorWindow>();
        }
    }

    //public class RenameLanguageWindow : EditorWindow
    //{

    //    public static RenameLanguageWindow Open(Vector2 position)
    //    {
    //        RenameLanguageWindow window = ScriptableObject.CreateInstance<RenameLanguageWindow>();
    //        window.ShowAsDropDown(new Rect(position, Vector2.one * 10), new Vector2(200, 50));
    //        return window;
    //    }

    //    public string languageName;
    //    public UnityAction<string> onFinished;

    //    private void OnGUI()
    //    {
    //        languageName = GUILayout.TextField(languageName);
    //        GUILayout.BeginHorizontal();
    //        if (GUILayout.Button("Finished"))
    //        {
    //            onFinished(languageName);
    //            Close();
    //        }
    //        GUILayout.EndHorizontal();
    //    }
    //}
}
