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
        private Text text;

        protected override void Awake()
        {
            text = GetComponent<Text>();
            base.Awake();
        }

        public override void Refresh()
        {
            text.text = L10nManager.Instance.GetText(Key);
        }
    }
}