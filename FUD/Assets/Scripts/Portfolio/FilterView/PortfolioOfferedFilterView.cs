using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioOfferedFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


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
        string gender = GetGenderType(filterCells[0].GetStatus());

        int sortId = filterCells[1].GetStatus();

        int orderBy = filterCells[2].GetStatus();

        int ageFrom = int.Parse(ageFromField.text);

        int ageTo = int.Parse(ageToField.text);

        GameManager.Instance.apiHandler.ApplyPortfolioOfferedFilter(sortId, orderBy, gender, ageFrom, ageTo, (status, resopnse) => {

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

    string GetGenderType(int index)
    {
        string gender = string.Empty;

        switch(index)
        {
            case 0:
                gender = "Male";
                break;

            case 1:
                gender = "Female";
                break;

            case 2:
                gender = "Others";
                break;
        }

        return gender;
    }
}
