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
using CZToolKit.Core.ObjectPool;

namespace CZToolKit.LocalizationText.Editor
{
    public class LanguageTreeViewItemPool : PoolBase<LanguageTreeViewItem>
    {
        protected override LanguageTreeViewItem CreateNewUnit()
        {
            return new LanguageTreeViewItem();
        }

        public override void Dispose()
        {

        }
    }
}