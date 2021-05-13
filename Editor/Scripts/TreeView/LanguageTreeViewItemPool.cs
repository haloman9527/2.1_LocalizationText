using CZToolKit.Core.ObjectPool;

namespace CZToolKit.LocalizationText.Editor
{
    public class LanguageTreeViewItemPool : PoolBase<LanguageTreeViewItem>
    {
        protected override LanguageTreeViewItem CreateNewUnit()
        {
            return new LanguageTreeViewItem();
        }
    }
}