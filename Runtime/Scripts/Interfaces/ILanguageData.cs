namespace Atom.L10n
{
    public interface ILanguageData
    {
        bool TryGetText(int key, out string text);
    }
}