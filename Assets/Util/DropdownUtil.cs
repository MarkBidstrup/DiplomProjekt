using System.Collections.Generic;
using TMPro;


/// <summary>
/// Utility class used to set dropdown menu options.
/// </summary>
public static class DropdownUtil
{
    /// <summary>
    /// Clears and sets dropdown menu options.
    /// </summary>
    /// <param name="options">The list of dropdown menu options.</param>
    /// <param name="dropdown">The dropdown menu.</param>
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

