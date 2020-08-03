using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioOfferedFilterView : MonoBehaviour
{
    public TMP_Dropdown genderDropdown;

    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;

    public TMP_InputField ageFromField;

    public TMP_InputField ageToField;


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

        int ageFrom = int.Parse(ageFromField.text);

        int ageTo = int.Parse(ageToField.text);

        GameManager.Instance.apiHandler.ApplyPortfolioOfferedFilter(sortId, orderById, genderDropdown.captionText.text, ageFrom, ageTo, (status, resopnse) => {

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
