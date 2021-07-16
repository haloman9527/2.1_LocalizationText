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
using System.Text.RegularExpressions;
using UnityEngine;
using CZToolKit.Core.Attributes;
using System.Text;

namespace CZToolKit.LocalizationText
{
    public abstract class LocalizationComponent : MonoBehaviour
    {
        [SerializeField]
        // [DrawTextFromKey(order = 999)]
        // [TextArea]
        [ReadOnly]
        protected string beforeText = "???";

        private const string MatchRegex = "(<#)([^\\s(<#|#>)]+)(#>)";
        private const string MatchRegex1 = "<LT>([^<|>]+)</LT>";
        private static Regex regex = new Regex(MatchRegex1);

        public string BeforeText
        {
            get { return beforeText; }
        }

        protected virtual void Start()
        {
            Refresh();
            LocalizationSystem.OnLanguageChanged += Refresh;
        }

        public virtual void SetText(string key)
        {
            beforeText = key;
            Refresh();
        }

        /// <summary> 解析并替换文本 </summary>
        public abstract void Refresh();

        private void OnDestroy()
        {
            LocalizationSystem.OnLanguageChanged -= Refresh;
        }

        public static string GetAfterText(string beforeText)
        {
            StringBuilder sb = new StringBuilder(beforeText);
            MatchCollection matchCollection = regex.Matches(beforeText);
            foreach (Match item in matchCollection)
            {
                if (LocalizationSystem.TryGetLocalisedValue(item.Groups[2].Value, out string value))
                    sb.Replace(item.Value, value);
            }

            return sb.ToString();
        }
    }
}