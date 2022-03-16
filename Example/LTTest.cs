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
using CZToolKit.LocalizationText;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LTTest : MonoBehaviour
{
    private Dropdown dropdown;
    public Button btnSetValue;
    public TextAsset textAsset;

    [LabelText("123")]
    public LocalizationText text;

    void Awake()
    {
        dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropDownValueChanged);
        LocalizationSystem.Init(textAsset.text, DataFormat.CSV);
        if (LocalizationSystem.TryGetLanguages(out string[] languages))
        {
            foreach (string item in languages)
            {
                dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(item) });
            }
            int index = 0;
            for (int i = 0; i < languages.Length; i++)
            {
                if (LocalizationSystem.Language == languages[i])
                {
                    index = i;
                    break;
                }
            }
            dropdown.value = index;
        }

        bool b = true;
        btnSetValue.onClick.AddListener(() =>
        {
            if (b)
                text.SetText("<LT>text2</LT>");
            else
                text.SetText("<LT>text1</LT>");
            b = !b;
        });
    }

    private void OnDropDownValueChanged(int i)
    {
        LocalizationSystem.SetLanguage(dropdown.options[i].text);
    }
}
