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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using UnityEngine;
using UnityEngine.UI;

namespace CZToolKit.I18N
{
    /// <summary> 此脚本用于处理Text组件的多文本 </summary>
    [RequireComponent(typeof(Text))]
    public sealed class LocalizationText : LocalizationComponent
    {
        private Text text;

        protected override void Awake()
        {
            text = GetComponent<Text>();
            ParseKey();
        }

        protected override void RefreshText(string text)
        {
            this.text.text = text;
        }
    }
}