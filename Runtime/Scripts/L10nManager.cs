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
using System.Collections.Generic;

namespace Moyo.L10n
{
    public class L10nManager : Singleton<L10nManager>
    {
        private Language language;
        private Language rollbackLanguage;
        private Func<Language, ILanguageData> languageLoader;
        private Dictionary<Language, ILanguageData> languageDatas = new Dictionary<Language, ILanguageData>();

        private HashSet<IL10n> l10nComponents = new HashSet<IL10n>();

        public Language Language
        {
            get => language;
            set => SetLanguage(value);
        }

        public void Init(Language rollbackLanguage, Func<Language, ILanguageData> languageLoader)
        {
            this.rollbackLanguage = rollbackLanguage;
            this.languageLoader = languageLoader;
            this.languageDatas[rollbackLanguage] = languageLoader(rollbackLanguage);
        }

        public void RegisterL10NComponent(IL10n l10n)
        {
            l10nComponents.Add(l10n);
        }

        public void UnRegisterL10NComponent(IL10n l10n)
        {
            l10nComponents.Remove(l10n);
        }

        public void SetLanguage(Language newLanguage)
        {
            if (!languageDatas.TryGetValue(newLanguage, out var data))
            {
                languageDatas[newLanguage] = data = languageLoader(newLanguage);
            }

            if (data == null)
            {
                newLanguage = rollbackLanguage;
            }

            if (this.language == newLanguage)
            {
                return;
            }

            this.language = newLanguage;

            foreach (var l10n in l10nComponents)
            {
                l10n.Refresh();
            }
        }

        public string GetText(int key)
        {
            languageDatas[language].TryGetText(key, out var text);
            return text;
        }
    }

    public interface ILanguageData
    {
        bool TryGetText(int key, out string text);
    }

    public interface IL10n
    {
        int Key { get; set; }

        void Refresh();
    }

    public struct SwitchLanguage
    {
        public Language language;

        public SwitchLanguage(Language language)
        {
            this.language = language;
        }
    }

    public class SwitchLanguageEvent : GlobalEvent<SwitchLanguage>
    {
        public override void Invoke(SwitchLanguage arg)
        {
            L10nManager.Instance?.SetLanguage(arg.language);
        }
    }
}