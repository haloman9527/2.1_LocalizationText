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

namespace CZToolKit.I18N
{
    /// <summary>
    /// 此脚本用于处理Text组件的多文本
    /// </summary>
    [RequireComponent(typeof(TextMesh))]
    public sealed class LocalizationTextMesh : LocalizationComponent
    {
        private TextMesh text;

        protected override void Awake()
        {
            text = GetComponent<TextMesh>();
            ParseKey();
        }

        protected override void RefreshText(string text)
        {
            this.text.text = text;
        }
    }
}