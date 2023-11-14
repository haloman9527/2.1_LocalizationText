using System;

namespace CZToolKit.I18N
{
    public interface ILocalization
    {
        event Action OnLanguageChanged;

        void SetLanguage(Language language);

        string GetText(string key);
    }
}