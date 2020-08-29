using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectOfferedFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetProjectProducers(1, (status, response) => {

            if (status)
            {
                List<ProjectOfferedModel> dataList = JsonUtility.FromJson<ProjectOfferedResponse>(response).data;

                List<DropdownModel> dropdownModels = new List<DropdownModel>();

                DropdownModel dropdownModel = null;

                for (int i = 0; i < dataList.Count; i++)
                {
                    dropdownModel = new DropdownModel();

                    dropdownModel.text = dataList[i].Projects.Users.name;

                    dropdownModel.id = dataList[i].Projects.Users.id;

                    dropdownModels.Add(dropdownModel);
                }

                filterCells[3].Load(dropdownModels);
            }
        });

        this.OnApplyFilter = OnApplyFilter;
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int genreId = filterCells[0].GetStatus();

        int sortId = filterCells[1].GetStatus();

        int orderBy = filterCells[2].GetStatus();

        int producerId = filterCells[3].GetStatus();

        GameManager.Instance.apiHandler.ApplyOfferedProjectsFilter(sortId, genreId, orderBy, producerId, (status, data) => {

            if (status)
            {
                OnApplyFilter?.Invoke(data.data);

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
