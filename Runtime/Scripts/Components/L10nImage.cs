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

namespace CZToolKit.L10N
{
    [RequireComponent(typeof(Image))]
    public sealed class L10nImage : L10nComponent
    {
        private Image img;

        protected override void Awake()
        {
            img = GetComponent<Image>();
            base.Awake();
        }

        public override async void Refresh()
        {
            var handle = ResourceManager.Instance.LoadAssetAsync<Sprite>(L10nManager.Instance.GetText(Key));
            await handle.Task;
            img.sprite = handle.Asset as Sprite;
        }
    }
}