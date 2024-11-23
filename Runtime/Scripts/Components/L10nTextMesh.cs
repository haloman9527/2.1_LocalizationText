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

namespace Jiange.L10N
{
    [RequireComponent(typeof(TextMesh))]
    public sealed class L10nTextMesh : L10nComponent
    {
        private TextMesh text;

        protected override void Awake()
        {
            base.Awake();
            text = GetComponent<TextMesh>();
        }

        public override void Refresh()
        {
            text.text = L10nManager.Instance.GetText(Key);
        }
    }
}