using System.Collections.Generic;
using UnityEngine;
using System;


public class AuditionViewedFilterView : MonoBehaviour
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

        int sortId = filterCells[0].GetStatus();

        int orderBy = filterCells[1].GetStatus();

        parameters.Add("source_from", 2);

        parameters.Add("sortBy", sortId);

        parameters.Add("sortOrder", orderBy);

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
