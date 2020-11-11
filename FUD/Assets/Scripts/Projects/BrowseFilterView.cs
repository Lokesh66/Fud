using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class BrowseFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


    public MultiSliderDragHandler dragHandler;

    public Slider ageSlider;


    public TextMeshProUGUI ageValueText;


    Action<List<PortfolioModel>> OnApplyFilter;

    float currentMaxAge = -1;



    public void Load(Action<List<PortfolioModel>> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        currentMaxAge = ageSlider.value;

        OnAgeSliderValueChange();
    }


    public void OnAgeSliderValueChange()
    {
        if (dragHandler.minAgeValue < ageSlider.value)
        {
            currentMaxAge = ageSlider.value;
        }
        else {
            ageSlider.value = currentMaxAge;
        }

        ageValueText.text =  (int)dragHandler.minAgeValue + "-" + (int)currentMaxAge + " Yrs";
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        string gender = GetGenderType(filterCells[0].GetStatus());

        int roleId = filterCells[1].GetStatus();

        GameManager.Instance.apiHandler.ApplyBrowseFilter((int)dragHandler.minAgeValue, (int)currentMaxAge, gender, roleId, (status, data) =>
        {
            if (status)
            {
                OnApplyFilter?.Invoke(data);

                OnCancelButtonAction();
            }
        });
    }

    void ClearData()
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

        switch (index)
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

