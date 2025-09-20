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

namespace Atom.L10n
{
    public class L10nManager : GameModuleSingleton<L10nManager>
    {
        private Language m_Language;
        private Language m_RollbackLanguage;
        private Func<Language, ILanguageData> m_LanguageLoader;
        private Dictionary<Language, ILanguageData> m_LanguageDatas;
        private HashSet<IL10n> m_Components;
        private IAssetLoader m_AssetLoader;

        public L10nManager(Language language, Language rollbackLanguage, Func<Language, ILanguageData> languageLoader)
        {
            m_RollbackLanguage = rollbackLanguage;
            m_LanguageLoader = languageLoader;
            m_LanguageDatas = new Dictionary<Language, ILanguageData>();
            m_Components = new HashSet<IL10n>(8);
            m_LanguageDatas[m_RollbackLanguage] = m_LanguageLoader(m_RollbackLanguage);
            SetLanguage(language);
        }

        public Language Language
        {
            get => m_Language;
            set => SetLanguage(value);
        }

        public IAssetLoader AssetLoader
        {
            get { return m_AssetLoader; }
        }

        public void SetAssetLoader(IAssetLoader assetLoader)
        {
            this.m_AssetLoader = assetLoader;
        }

        public void Register(IL10n component)
        {
            m_Components.Add(component);
        }

        public void Unregister(IL10n component)
        {
            m_Components.Remove(component);
        }

        public void SetLanguage(Language language)
        {
            if (m_Language == language)
                return;
            m_LanguageDatas.Remove(language);
            m_Language = language;
            if (!m_LanguageDatas.ContainsKey(m_Language))
                m_LanguageDatas[m_Language] = m_LanguageLoader(language);

            foreach (var l10n in m_Components)
            {
                l10n.Refresh();
            }
        }

        public string GetText(int key)
        {
            var language = m_LanguageDatas[m_Language];
            var rollbackLanguage = m_LanguageDatas[m_RollbackLanguage];
            if (language.TryGetText(key, out var text))
                return text;
            else if (rollbackLanguage.TryGetText(key, out var rollbackText))
                return rollbackText;
            return "undefined text";
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
}