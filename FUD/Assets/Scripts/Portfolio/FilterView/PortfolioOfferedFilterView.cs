using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioOfferedFilterView : MonoBehaviour
{
    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;


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


        GameManager.Instance.apiHandler.ApplyPortfolioOfferedFilter(sortId, orderById, (status, resopnse) => {

            if (status)
            {
                PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(resopnse);

                OnApplyFilter?.Invoke(responseModel.data);

                OnCancelButtonAction();
            }   
        });
    }

    void ClearData()
    {
        orderDropdown.value = sortDropdown.value = 0;
    }
}
