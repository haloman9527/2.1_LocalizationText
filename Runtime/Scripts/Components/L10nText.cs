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

using UnityEngine;
using UnityEngine.UI;

namespace Atom.L10n
{
    [RequireComponent(typeof(Text))]
    public sealed class L10nText : L10nComponent
    {
        private Text m_Component;

        protected override void Awake()
        {
            m_Component = GetComponent<Text>();
            base.Awake();
        }

        public override void Refresh()
        {
            m_Component.text = L10nManager.Instance.GetText(Key);
        }
    }
}