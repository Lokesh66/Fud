using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class BrowseFilterView : MonoBehaviour
{
    public RectTransform minFillTrans;

    public Slider ageSlider;

    public TMP_InputField ageFromField;

    public TMP_InputField ageToField;

    public TMP_Dropdown genderDropdown;

    public TMP_Dropdown roleDropdown;

    public TextMeshProUGUI ageValueText;


    List<Genre> genres;

    Action<List<PortfolioModel>> OnApplyFilter;

    float minAgeValue => ageSlider.minValue;


    public void Load(Action<List<PortfolioModel>> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        SetView();
    }

    void SetView()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roleDropdown.ClearOptions();

        roleDropdown.AddOptions(options);
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
        string selectedGenreText = roleDropdown.options[roleDropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        GameManager.Instance.apiHandler.ApplyBrowseFilter(ageFromField.text, ageToField.text, genderDropdown.captionText.text, selectedGenre.id, (status, data) => {

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

        genderDropdown.value = 0;
    }
}
