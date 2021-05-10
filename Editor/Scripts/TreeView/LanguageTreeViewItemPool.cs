using CZToolKit.Core.ObjectPool;

namespace CZToolKit.LocalizationText.Editor
{
    public class LanguageTreeViewItemPool : PoolBase<LanguageTreeViewItem>
    {
        /// <summary> 回收所有 </summary>
        public override void RecycleAll()
        {
            IdleList.AddRange(WorkList);
            WorkList.Clear();
        }

        protected override LanguageTreeViewItem CreateNewUnit() { return new LanguageTreeViewItem(); }

        protected override void OnRecycle(LanguageTreeViewItem unit) { base.OnRecycle(unit); }
    }
}