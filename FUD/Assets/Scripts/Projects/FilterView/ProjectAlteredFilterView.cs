using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectAlteredFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


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
        int statusId = filterCells[0].GetStatus();

        int sortId = filterCells[1].GetStatus();

        int orderById = filterCells[2].GetStatus();

        GameManager.Instance.apiHandler.ApplyAlteredProjectsFilter(statusId, sortId, orderById, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data.data);

                OnCancelButtonAction();
            }   
        });
    }

    public void ClearData()
    {
        for (int i = 0; i < filterCells.Count; i++)
        {
            filterCells[i].ClearSelectedModels();
        }
    }
}
