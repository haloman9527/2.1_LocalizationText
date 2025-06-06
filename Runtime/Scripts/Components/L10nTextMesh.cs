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

namespace Atom.L10n
{
    [RequireComponent(typeof(TextMesh))]
    public sealed class L10nTextMesh : L10nComponent
    {
        private TextMesh m_Component;

        protected override void Awake()
        {
            base.Awake();
            m_Component = GetComponent<TextMesh>();
        }

        public override void Refresh()
        {
            m_Component.text = L10nManager.Instance.GetText(Key);
        }
    }
}