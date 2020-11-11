using System.Collections.Generic;
using UnityEngine;
using System;


public class AuditionOfferedFilterView : MonoBehaviour
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
        int sortId = filterCells[0].GetStatus();

        int orderById = filterCells[1].GetStatus();


        GameManager.Instance.apiHandler.ApplyAudtionOfferedFilter(sortId, orderById, (status, response) => {

            if (status)
            {
                AuditionsResponse responseModel = JsonUtility.FromJson<AuditionsResponse>(response);

                OnApplyFilter?.Invoke(responseModel.data);

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
