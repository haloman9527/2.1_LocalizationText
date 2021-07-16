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
using System.Linq;
using System.Collections.Generic;

namespace CZToolKit.LocalizationText
{
    public class LocalizationData
    {
        private Dictionary<string, int> languageDic;
        private Dictionary<string, string[]> dataTable;

        public Dictionary<string, int> Languages
        {
            get { return languageDic; }
        }

        public Dictionary<string, string[]> DataTable
        {
            get { return dataTable; }
        }

        public LocalizationData(Dictionary<string, int> languages, Dictionary<string, string[]> dataTable)
        {
            this.languageDic = languages;
            this.dataTable = dataTable;
        }

        public bool TryGetLanguages(out string[] languages)
        {
            languages = this.languageDic.Keys.ToArray();
            return true;
        }

        public bool TryGetValue(string language, string key, out string value)
        {
            if (languageDic.TryGetValue(language, out int languageIndex) && dataTable.TryGetValue(key, out string[] values))
            {
                value = values[languageIndex];
                return true;
            }
            else
            {
                value = key;
                return false;
            }
        }
    }

    public class LocalizationSystem
    {
        private static LocalizationData LocalizationData;
        public static Action OnLanguageChanged;
        public static bool Initialized { get; private set; }
        public static string Language { get; private set; }

        public static void Init(string configText, DataFormat dataFormat, string language = null)
        {
            if (Initialized)
                return;
            Initialized = true;
            LocalizationData = Load(configText, dataFormat);

            Language = "";
            //Set default language in here
            //在这里设置初始化默认语言
            if (string.IsNullOrEmpty(language) && LocalizationData.Languages.Count > 0)
            {
                KeyValuePair<string, int> k = LocalizationData.Languages.First();
                if (LocalizationData.Languages.ContainsKey(k.Key))
                    Language = k.Key;
            }
            else
                Language = language;
            OnLanguageChanged?.Invoke();
        }

        public static bool TryGetLanguages(out string[] languages)
        {
            if (!Initialized)
            {
                languages = new string[0];
                return false;
            }

            languages = LocalizationData.Languages.Keys.ToArray();
            return true;
        }

        public static bool TryGetLocalisedValue(string key, out string value)
        {
            if (!Initialized)
            {
                value = key;
                return false;
            }

            if (LocalizationData.Languages.TryGetValue(Language, out int languageIndex) && LocalizationData.DataTable.TryGetValue(key, out string[] values))
            {
                value = values[languageIndex];
                return true;
            }
            else
            {
                value = key;
                return false;
            }
        }

        public static LocalizationData Load(string configText, DataFormat dataFormat)
        {
            if (string.IsNullOrEmpty(configText))
                return null;

            Dictionary<string, int> languages;
            Dictionary<string, string[]> valueTable;
            switch (dataFormat)
            {
                case DataFormat.Json:
                    LoadWithJson(configText, out languages, out valueTable);
                    break;
                case DataFormat.CSV:
                    LoadWithCSV(configText, out languages, out valueTable);
                    break;
                default:
                    languages = new Dictionary<string, int>();
                    valueTable = new Dictionary<string, string[]>();
                    break;
            }
            LocalizationData data = new LocalizationData(languages, valueTable);
            return data;
        }

        public static void SetLanguage(string language)
        {
            if (Language != language)
            {
                Language = language;
                OnLanguageChanged?.Invoke();
            }
        }

        private static void LoadWithJson(string dataText, out Dictionary<string, int> languages, out Dictionary<string, string[]> valueTable)
        {
            languages = new Dictionary<string, int>();
            valueTable = new Dictionary<string, string[]>();
            int languageCount = 0;
            string[][] table = Newtonsoft.Json.JsonConvert.DeserializeObject<string[][]>(dataText);
            languageCount = table[0].Length - 1;
            for (int i = 1; i < table[0].Length; i++)
            {
                languages[table[0][i]] = i - 1;
            }

            for (int line = 1; line < table.Length; line++)
            {
                string[] newValues = new string[languageCount + 1];
                for (int f = 0; f < newValues.Length; f++)
                {
                    if (f < table[line].Length)
                        newValues[f] = table[line][f];
                    else
                        newValues[f] = "";
                }

                valueTable[newValues[0]] = newValues.Skip(1).ToArray();
            }
        }

        private static void LoadWithCSV(string dataText, out Dictionary<string, int> languages, out Dictionary<string, string[]> valueTable)
        {
            int languageCount = 0;
            Dictionary<string, int> tempLanguages = new Dictionary<string, int>();
            Dictionary<string, string[]> tempValueTable = new Dictionary<string, string[]>();
            bool langs = false;
            CSVLoader.DeserializeTable(dataText, (values) =>
            {
                if (!langs)
                {
                    languageCount = values.Length - 1;
                    for (int i = 1; i < values.Length; i++)
                    {
                        tempLanguages[values[i]] = i - 1;
                    }
                    langs = true;
                }
                else
                {
                    string[] newValues = new string[languageCount + 1];
                    for (int i = 0; i < newValues.Length; i++)
                    {
                        if (i < values.Length)
                            newValues[i] = values[i];
                        else
                            newValues[i] = "";
                    }
                    tempValueTable[newValues[0]] = newValues.Skip(1).ToArray();
                }
            });
            languages = tempLanguages;
            valueTable = tempValueTable;
        }
    }

    public enum DataFormat
    {
        Json, CSV
    }
}