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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion

using System;
using UnityEngine;

namespace CZToolKit.I18N
{
    public abstract class LocalizationComponent : MonoBehaviour
    {
        [SerializeField]
        private string key;

        public string Key
        {
            get { return key; }
            set
            {
                if (string.Equals(key, value))
                    return;
                key = value;
                ParseKey();
            }
        }

        protected virtual void Awake()
        {
            Localization.Instance.OnLanguageChanged += ParseKey;
            ParseKey();
        }

        protected virtual void OnDestroy()
        {
            Localization.Instance.OnLanguageChanged -= ParseKey;
        }

        protected void ParseKey()
        {
            var text = Localization.Instance.GetText(key);
            if (string.IsNullOrEmpty(text))
                text = key;
            RefreshText(text);
        }
        
        protected abstract void RefreshText(string text);
    }
}