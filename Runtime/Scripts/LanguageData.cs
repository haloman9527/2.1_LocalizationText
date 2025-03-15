using System.Collections.Generic;

namespace Atom.L10n
{
    public class LanguageData : ILanguageData
    {
        public Dictionary<int, string> data = new Dictionary<int, string>();

        public bool TryGetText(int key, out string text)
        {
            return data.TryGetValue(key, out text);
        }
    }
}