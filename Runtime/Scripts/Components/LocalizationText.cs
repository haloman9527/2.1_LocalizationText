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
using UnityEngine;
using UnityEngine.UI;

namespace CZToolKit.LocalizationText
{
    /// <summary> 此脚本用于处理Text组件的多文本 </summary>
    [RequireComponent(typeof(Text))]
    public sealed class LocalizationText : LocalizationComponent
    {
        private Text text;

        void Awake()
        {
            text = GetComponent<Text>();
            SetText(text.text);
        }

        public override void SetText(string _text)
        {
            beforeText = _text;
            Refresh();
        }

        public override void Refresh()
        {
            text.text = GetAfterText(beforeText);
        }
    }
}