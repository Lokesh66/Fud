using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class StoryBrowseFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        SetStoryDropdownView();
    }

    void SetStoryDropdownView()
    {
        GameManager.Instance.apiHandler.GetStoryTypes((status, response) => {

            StoryTypesResponse responseModel = JsonUtility.FromJson<StoryTypesResponse>(response);

            if (status)
            {
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

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int genreId = filterCells[0].GetStatus();

        int storyTypeId = filterCells[1].GetStatus();

        int sortId = filterCells[2].GetStatus();

        int orderById = filterCells[3].GetStatus();

        GameManager.Instance.apiHandler.ApplyStoryBrowseFilter(genreId, storyTypeId, sortId, orderById, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data);

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
}
