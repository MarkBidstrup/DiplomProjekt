using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // TODO: Handle nulls 
    private void Awake()
    {
        modelController = GameObject.FindWithTag("ModelController").GetComponent<ModelController>();
        selectIssue.onValueChanged.AddListener(OnDropdownValueChanged);
        AssignedTo.onValueChanged.AddListener(OnAssignedToValueChanged);
        DropdownUtil.SetDropdownOptions(GetIssueSubjects(), selectIssue);
        DropdownUtil.SetDropdownOptions(AssignedToOptions.Options, AssignedTo);
        TMP_Dropdown.OptionData dropdownTemplateText = new TMP_Dropdown.OptionData("Select Issue");
        selectIssue.options.Insert(0, dropdownTemplateText);
    }

    private List<string> GetIssueSubjects()
    {
        if (modelController.GetModel() == null)
        {
            return null;
        }
        List<Issue> flagList = modelController.GetModel().Issues;
        return flagList.Select(flag => flag.Subject).ToList();
    }

    private void OnDropdownValueChanged(int index)
    {
        int indexWithOffset = index - 1; // Offset needed because of template option
        List<Issue> flagList = modelController.GetModel().Issues;
        dueDate.text = flagList[indexWithOffset].DueDate;
        description.text = flagList[indexWithOffset].Description;
        int assigneeIndex = AssignedToOptions.Options.IndexOf(flagList[indexWithOffset].AssignedTo);
        AssignedTo.value = assigneeIndex;
    }

    private void OnAssignedToValueChanged(int index)
    {
        List<Issue> flagList = modelController.GetModel().Issues;

        int selectedIssueIndex = selectIssue.value - 1; // Offset needed because of template option
        if (selectedIssueIndex >= 0 && selectedIssueIndex < flagList.Count)
        {
            flagList[selectedIssueIndex].AssignedTo = AssignedToOptions.Options[index];
        }
    }
}
