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

namespace CZToolKit.LocalizationText
{
    /// <summary>
    /// 此脚本用于处理Text组件的多文本
    /// </summary>
    [RequireComponent(typeof(TextMesh))]
    public sealed class LocalizationTextMesh : LocalizationComponent
    {
        private TextMesh text;

        void Awake()
        {
            text = GetComponent<TextMesh>();
            SetText(text.text);
        }

        /// <summary>
        /// 参数是key
        /// </summary>
        /// <param name="key"></param>
        public override void SetText(string key)
        {
            base.beforeText = key;
            Refresh();
        }

        public override void Refresh()
        {
            if (LocalizationSystem.TryGetLocalisedValue(beforeText,out string value))
                text.text = value;
        }
    }
}