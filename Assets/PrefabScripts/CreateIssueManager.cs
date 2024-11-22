using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateIssueManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        DropdownUtil.SetDropdownOptions(AssignedToOptions.Options, dropdown);
    }
}
