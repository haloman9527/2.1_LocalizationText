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
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public sealed class L10nImage : L10nComponent
    {
        private Image m_Component;

        protected override void Awake()
        {
            base.Awake();
            m_Component = GetComponent<Image>();
        }

        public override async void Refresh()
        {
            var handle = L10nManager.Instance.AssetLoader.LoadAssetAsync<Sprite>(L10nManager.Instance.GetText(Key));
            await handle.Task;
            m_Component.sprite = handle.Asset as Sprite;
        }
    }
}