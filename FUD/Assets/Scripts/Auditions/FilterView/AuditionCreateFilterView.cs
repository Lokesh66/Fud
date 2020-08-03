using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class AuditionCreateFilterView : MonoBehaviour
{
    public TMP_Dropdown projectDropdown;

    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;


    List<AuditionProjectModel> projectsModels;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        SetProjectDropdownView();
    }

    void SetProjectDropdownView()
    {
        GameManager.Instance.apiHandler.GetProjectAuditions((status, dataList) =>{

            if (status)
            {
                projectsModels = dataList;

                List<string> options = new List<string>();

                foreach (var option in projectsModels)
                {
                    options.Add(option.title);
                }

                projectDropdown.ClearOptions();
                projectDropdown.AddOptions(options);
            }
        });
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int sortId = sortDropdown.value;

        int orderById = orderDropdown.value + 1;

        int projectId = projectsModels[projectDropdown.value].id;

        GameManager.Instance.apiHandler.ApplyAudtionAlteredFilter(sortId, projectId, orderById, (status, response) => {

            if (status)
            {
                JoinedAuditionsResponse responseModel = JsonUtility.FromJson<JoinedAuditionsResponse>(response);

                OnApplyFilter?.Invoke(responseModel.data);

                OnCancelButtonAction();
            }
        });
    }

    void ClearData()
    {
        projectDropdown.value = 0;
    }
}
