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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CZToolKit.LocalizationText.Editor
{
    [CustomPropertyDrawer(typeof(DrawTextFromKeyAttribute))]
    public class DrawTextFromKeyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= 34;
            base.OnGUI(position, property, label);
            position.x += position.width;
            position.width = 34;
            if (GUI.Button(position, new GUIContent(EditorGUIUtility.FindTexture("Search Icon"))))
            {
                EditorWindow.GetWindow<LocalizationEditorWindow>();
            }
        }
    }
}
#endif