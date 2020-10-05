using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioOfferedFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;

    public FilterCell categeryFilterCell;


    public TextMeshProUGUI ageValueText;


    public MultiSliderDragHandler dragHandler;

    public Slider ageSlider;


    float currentMaxAge = -1;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        filterCells[0].OnRoleSelection(OnRoleSelectAction);
    }

    void OnRoleSelectAction(object selectedRole)
    {
        Craft roleModel = selectedRole as Craft;

        GameManager.Instance.apiHandler.GetRoleCategeries(roleModel.id, (status, response) => {

            if (status)
            {
                RoleCategeryResponse responseModel = JsonUtility.FromJson<RoleCategeryResponse>(response);

                if (responseModel == null)
                {
                    return;
                }

                categeryFilterCell.gameObject.SetActive(true);

                List<DropdownModel> dropdownModels = new List<DropdownModel>();

                DropdownModel dropdownModel = null;

                for (int i = 0; i < responseModel.data.Count; i++)
                {
                    dropdownModel = new DropdownModel();

                    dropdownModel.text = responseModel.data[i].name;

                    dropdownModel.id = responseModel.data[i].id;

                    dropdownModels.Add(dropdownModel);
                }

                filterCells[1].Load(dropdownModels);
            }
        });
    }

    public void OnAgeSliderValueChange()
    {
        if (dragHandler.minAgeValue < ageSlider.value)
        {
            currentMaxAge = ageSlider.value;
        }
        else
        {
            ageSlider.value = currentMaxAge;
        }

        ageValueText.text = (int)dragHandler.minAgeValue + "-" + (int)currentMaxAge + " Yrs";
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        categeryFilterCell.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int roleId = filterCells[0].GetStatus();

        int roleCategeryId = filterCells[1].GetStatus();

        int sortId = filterCells[2].GetStatus();

        int orderBy = filterCells[3].GetStatus();

        int ageFrom = (int)dragHandler.minAgeValue;

        int ageTo = (int)ageSlider.value;

        GameManager.Instance.apiHandler.ApplyPortfolioOfferedFilter(sortId, orderBy, roleId, roleCategeryId, ageFrom, ageTo, (status, resopnse) => {

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
        currentMaxAge = 100;

        dragHandler.ClearData();

        ageSlider.value = currentMaxAge;

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
