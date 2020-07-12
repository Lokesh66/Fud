using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class PortfolioAlteredFilterView : MonoBehaviour
{
    public TMP_Dropdown statusDropdown;

    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;

    public TMP_Dropdown roleDropdown;


    List<Genre> genres;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
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

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        string selectedGenreText = roleDropdown.options[roleDropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        int sortId = sortDropdown.value;

        int orderById = orderDropdown.value + 1;

        int statusId = GetStatusId();

        GameManager.Instance.apiHandler.ApplyPortfolioAlteredFilter(selectedGenre.id, statusId, sortId, orderById, (status, resopnse) => {

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
        statusDropdown.value = 0;
    }

    int GetStatusId()
    {
        int statusId = -1;

        switch (statusDropdown.value)
        {
            case 0:
                statusId = 5;
                break;
            case 1:
                statusId = 3;
                break;
            case 2:
                statusId = 8;
                break;
            case 3:
                statusId = 2;
                break;
        }

        return statusId;
    }
}
