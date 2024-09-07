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
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using TMPro;
using UnityEngine;

namespace CZToolKit.L10N
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class L10nTMP : L10nComponent
    {
        private TextMeshProUGUI text;

        protected override void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            base.Awake();
        }

        public override void Refresh()
        {
            text.text = L10nManager.Instance.GetText(Key);
        }
    }
}