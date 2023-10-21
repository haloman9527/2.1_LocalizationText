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
 *  Blog: https://www.crosshair.top/
 *
 */

#endregion

using System;
using System.Collections.Generic;
using CZToolKit.Config;
using CZToolKit.Singletons;
using UnityEngine;

namespace CZToolKit.I18N
{
    public class Localization : Singleton<Localization>, ISingletonAwake
    {
        private Language language;
        private Dictionary<string, string> data = new Dictionary<string, string>();

        public Action OnLanguageChanged;

        public void Awake()
        {
            this.language = (Language)ConfigManager.Instance.GetInt("Language", (int)GetSystemLanguage());
        }

        public Language GetSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case UnityEngine.SystemLanguage.Afrikaans: return Language.Afrikaans;
                case UnityEngine.SystemLanguage.Arabic: return Language.Arabic;
                case UnityEngine.SystemLanguage.Basque: return Language.Basque;
                case UnityEngine.SystemLanguage.Belarusian: return Language.Belarusian;
                case UnityEngine.SystemLanguage.Bulgarian: return Language.Bulgarian;
                case UnityEngine.SystemLanguage.Catalan: return Language.Catalan;
                case UnityEngine.SystemLanguage.Chinese: return Language.ChineseSimplified;
                case UnityEngine.SystemLanguage.ChineseSimplified: return Language.ChineseSimplified;
                case UnityEngine.SystemLanguage.ChineseTraditional: return Language.ChineseTraditional;
                case UnityEngine.SystemLanguage.Czech: return Language.Czech;
                case UnityEngine.SystemLanguage.Danish: return Language.Danish;
                case UnityEngine.SystemLanguage.Dutch: return Language.Dutch;
                case UnityEngine.SystemLanguage.English: return Language.English;
                case UnityEngine.SystemLanguage.Estonian: return Language.Estonian;
                case UnityEngine.SystemLanguage.Faroese: return Language.Faroese;
                case UnityEngine.SystemLanguage.Finnish: return Language.Finnish;
                case UnityEngine.SystemLanguage.French: return Language.French;
                case UnityEngine.SystemLanguage.German: return Language.German;
                case UnityEngine.SystemLanguage.Greek: return Language.Greek;
                case UnityEngine.SystemLanguage.Hebrew: return Language.Hebrew;
                case UnityEngine.SystemLanguage.Hungarian: return Language.Hungarian;
                case UnityEngine.SystemLanguage.Icelandic: return Language.Icelandic;
                case UnityEngine.SystemLanguage.Indonesian: return Language.Indonesian;
                case UnityEngine.SystemLanguage.Italian: return Language.Italian;
                case UnityEngine.SystemLanguage.Japanese: return Language.Japanese;
                case UnityEngine.SystemLanguage.Korean: return Language.Korean;
                case UnityEngine.SystemLanguage.Latvian: return Language.Latvian;
                case UnityEngine.SystemLanguage.Lithuanian: return Language.Lithuanian;
                case UnityEngine.SystemLanguage.Norwegian: return Language.Norwegian;
                case UnityEngine.SystemLanguage.Polish: return Language.Polish;
                case UnityEngine.SystemLanguage.Portuguese: return Language.PortuguesePortugal;
                case UnityEngine.SystemLanguage.Romanian: return Language.Romanian;
                case UnityEngine.SystemLanguage.Russian: return Language.Russian;
                case UnityEngine.SystemLanguage.SerboCroatian: return Language.SerboCroatian;
                case UnityEngine.SystemLanguage.Slovak: return Language.Slovak;
                case UnityEngine.SystemLanguage.Slovenian: return Language.Slovenian;
                case UnityEngine.SystemLanguage.Spanish: return Language.Spanish;
                case UnityEngine.SystemLanguage.Swedish: return Language.Swedish;
                case UnityEngine.SystemLanguage.Thai: return Language.Thai;
                case UnityEngine.SystemLanguage.Turkish: return Language.Turkish;
                case UnityEngine.SystemLanguage.Ukrainian: return Language.Ukrainian;
                case UnityEngine.SystemLanguage.Unknown: return Language.Unspecified;
                case UnityEngine.SystemLanguage.Vietnamese: return Language.Vietnamese;
                default: return Language.Unspecified;
            }
        }

        public string GetText(string key)
        {
            data.TryGetValue(key, out var text);
            return text;
        }
    }
}