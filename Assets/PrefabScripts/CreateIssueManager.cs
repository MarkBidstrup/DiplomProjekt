using TMPro;
using UnityEngine;


/// <summary>
/// Manages the assigned to dropdown menu options of the create issue UI.
/// </summary>
public class CreateIssueManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        DropdownUtil.SetDropdownOptions(AssignedToOptions.Options, dropdown);
    }
}
