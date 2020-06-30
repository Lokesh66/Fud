using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectAlteredFilterView : MonoBehaviour
{
    public TMP_Dropdown statusDropdown;

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

        int statusId = GetStatusId();

        GameManager.Instance.apiHandler.ApplyAlteredProjectsFilter(statusId, selectedGenre.id, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data);

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
