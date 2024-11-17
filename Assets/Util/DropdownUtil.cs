using System;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class DropdownUtil
{
    public static void SetDropdownOptions(List<string> options, TMP_Dropdown dropdown)
    {
        if (options == null)
        {
            return;
        }
        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        foreach (string option in options)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(option));
        }
        dropdown.AddOptions(dropdownOptions);
    }
}

