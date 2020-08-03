using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectAlteredFilterView : MonoBehaviour
{
    public TMP_Dropdown statusDropdown;

    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;


    List<Genre> genres;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;
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

        int statusId = GetStatusId();

        GameManager.Instance.apiHandler.ApplyAlteredProjectsFilter(statusId, sortId, orderById, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data.data);

                OnCancelButtonAction();
            }   
        });
    }

    void ClearData()
    {
        statusDropdown.value = 0;
    }

    int GetStatusId()
    {
        int statusId = -1;

        switch (statusDropdown.value)
        {
            case 0:
                statusId = 5;
                break;
            case 1:
                statusId = 3;
                break;
            case 2:
                statusId = 8;
                break;
            case 3:
                statusId = 2;
                break;
        }

        return statusId;
    }
}
