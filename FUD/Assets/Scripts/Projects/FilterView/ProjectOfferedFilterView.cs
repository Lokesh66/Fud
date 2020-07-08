using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectOfferedFilterView : MonoBehaviour
{
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

        GameManager.Instance.apiHandler.ApplyOfferedProjectsFilter(sortId, selectedGenre.id, orderById, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data);

                OnCancelButtonAction();
            }   
        });
    }

    void ClearData()
    {
        orderDropdown.value = sortDropdown.value = 0;
    }
}
