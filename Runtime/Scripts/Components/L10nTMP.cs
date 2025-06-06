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

namespace Atom.L10n
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class L10nTMP : L10nComponent
    {
        private TextMeshProUGUI m_Component;

        protected override void Awake()
        {
            base.Awake();
            m_Component = GetComponent<TextMeshProUGUI>();
        }

        public override void Refresh()
        {
            m_Component.text = L10nManager.Instance.GetText(Key);
        }
    }
}