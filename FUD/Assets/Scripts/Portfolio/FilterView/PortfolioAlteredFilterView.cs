using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioAlteredFilterView : MonoBehaviour
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
        int genreId = filterCells[0].GetStatus();

        int sortId = filterCells[1].GetStatus();

        int orderById = filterCells[2].GetStatus();

        int statusId = filterCells[3].GetStatus();

        GameManager.Instance.apiHandler.ApplyPortfolioAlteredFilter(genreId, statusId, sortId, orderById, (status, resopnse) => {

            if (status)
            {
                PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(resopnse);

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
