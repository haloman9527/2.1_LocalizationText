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
    [RequireComponent(typeof(RawImage))]
    public sealed class L10nRawImage : L10nComponent
    {
        private RawImage m_Component;

        protected override void Awake()
        {
            m_Component = GetComponent<RawImage>();
            base.Awake();
        }

        public override async void Refresh()
        {
            var handle = L10nManager.Instance.AssetLoader.LoadAssetAsync<Texture>(L10nManager.Instance.GetText(Key));
            await handle.Task;
            m_Component.texture = handle.Asset as Texture;
        }
    }
}