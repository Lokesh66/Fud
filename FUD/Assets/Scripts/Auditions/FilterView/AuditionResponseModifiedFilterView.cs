using System.Collections.Generic;
using UnityEngine;
using System;


public class AuditionResponseModifiedFilterView : MonoBehaviour
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
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        int statusId = filterCells[0].GetStatus();

        int sortId = filterCells[1].GetStatus();

        int orderById = filterCells[2].GetStatus();

        parameters.Add("status", statusId);

        parameters.Add("source_from", 1);

        parameters.Add("sortBy", sortId);

        parameters.Add("sortOrder", orderById);

        GameManager.Instance.apiHandler.SearchAuditions(1, parameters, (status, data) => {

            if (status)
            {
                SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(data);

                OnApplyFilter?.Invoke(auditionResponse.data);

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
