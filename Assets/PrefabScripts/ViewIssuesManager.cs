using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


/// <summary>
/// Manages select issue dropdown menu options and assigned to dropdown menu options of the view issues UI.
/// </summary>
public class ViewIssuesManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown selectIssue;
    [SerializeField]
    private TMP_InputField dueDate;
    [SerializeField]
    private TMP_Dropdown AssignedTo;
    [SerializeField]
    private TMP_InputField description;

    private ModelController modelController;

    /// <summary>
    /// Manages select issue dropdown menu options and assigned to dropdown menu options and inserts template text to the first select issue dropdown menu option.
    /// </summary>
    private void Awake()
    {
        modelController = ModelController.Instance;
        selectIssue.onValueChanged.AddListener(OnDropdownValueChanged);
        DropdownUtil.SetDropdownOptions(GetIssueSubjects(), selectIssue);
        DropdownUtil.SetDropdownOptions(AssignedToOptions.Options, AssignedTo);
        TMP_Dropdown.OptionData dropdownTemplateText = new TMP_Dropdown.OptionData("Select Issue");
        selectIssue.options.Insert(0, dropdownTemplateText);
    }

    /// <summary>
    /// Gets the issue subject string values.
    /// </summary>
    /// <returns>A list with issue subject string values</returns>
    private List<string> GetIssueSubjects()
    {
        if (modelController.GetModel() == null)
        {
            return null;
        }
        List<Issue> issueList = modelController.GetModel().Issues;
        return issueList.Select(flag => flag.Subject).ToList();
    }

    /// <summary>
    /// Updates input field text when the string value of the select issue dropdown menu option has changed by option index.
    /// </summary>
    /// <param name="index">The select issue dropdown menu option index</param>
    private void OnDropdownValueChanged(int index)
    {
        int indexWithOffset = index - 1; // Offset needed because of template option
        List<Issue> flagList = modelController.GetModel().Issues;
        dueDate.text = flagList[indexWithOffset].DueDate;
        description.text = flagList[indexWithOffset].Description;
        int assigneeIndex = AssignedToOptions.Options.IndexOf(flagList[indexWithOffset].AssignedTo);
        AssignedTo.value = assigneeIndex;
    }
}
