using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class BrowseFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


    public RectTransform minFillTrans;

    public Slider ageSlider;

    public TMP_InputField ageFromField;

    public TMP_InputField ageToField;

    public TextMeshProUGUI ageValueText;


    List<Craft> crafts;

    Action<List<PortfolioModel>> OnApplyFilter;

    float minAgeValue => ageSlider.minValue;


    public void Load(Action<List<PortfolioModel>> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;
    }

 
    public void OnAgeSliderValueChange()
    {
        ageValueText.text = minAgeValue + "-" + ageSlider.value + " Yrs";
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

        GameManager.Instance.apiHandler.ApplyBrowseFilter(ageFromField.text, ageToField.text, gender, roleId, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data);

                OnCancelButtonAction();
            }   
        });
    }

    void ClearData()
    {
        ageFromField.text = 3.ToString();

        ageToField.text = 100.ToString();

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
