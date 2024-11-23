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

namespace Jiange.L10N
{
    [RequireComponent(typeof(RawImage))]
    public sealed class L10nRawImage : L10nComponent
    {
        private RawImage img;

        protected override void Awake()
        {
            img = GetComponent<RawImage>();
            base.Awake();
        }

        public override async void Refresh()
        {
            var handle = ResourceManager.Instance.LoadAssetAsync<Texture>(L10nManager.Instance.GetText(Key));
            await handle.Task;
            img.texture = handle.Asset as Texture;
        }
    }
}