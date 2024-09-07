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

namespace CZToolKit.L10N
{
    public abstract class L10nComponent : MonoBehaviour, IL10n
    {
        [SerializeField] private int key;

        public int Key
        {
            get => key;
            set
            {
                if (key == value)
                {
                    return;
                }

                key = value;
                Refresh();
            }
        }

        protected virtual void Awake()
        {
            L10nManager.Instance.RegisterL10NComponent(this);
            Refresh();
        }

        protected virtual void OnDestroy()
        {
            L10nManager.Instance.UnRegisterL10NComponent(this);
        }

        public abstract void Refresh();
    }
}