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

using System;
using UnityEngine;

namespace Atom.L10n
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
            if (L10nManager.Instance != null)
            {
                L10nManager.Instance.RegisterL10NComponent(this);
            }
        }

        private void Start()
        {
            if (L10nManager.Instance != null)
            {
                Refresh();
            }
        }

        protected virtual void OnDestroy()
        {
            if (L10nManager.Instance != null)
            {
                L10nManager.Instance.UnRegisterL10NComponent(this);
            }
        }

        public abstract void Refresh();
    }
}