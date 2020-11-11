using System.Collections.Generic;
using UnityEngine;
using System;


public class AuditionCreateFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        SetProjectDropdownView();
    }

    void SetProjectDropdownView()
    {
        GameManager.Instance.apiHandler.GetProjectAuditions((status, dataList) =>{

            if (status)
            {
                List<DropdownModel> dropdownModels = new List<DropdownModel>();

                DropdownModel dropdownModel = null;

                for (int i = 0; i < dataList.Count; i++)
                {
                    dropdownModel = new DropdownModel();

                    dropdownModel.text = dataList[i].title;

                    dropdownModel.id = dataList[i].id;

                    dropdownModels.Add(dropdownModel);
                }

                filterCells[0].Load(dropdownModels);
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
        int projectId = filterCells[0].GetStatus();

        int sortId = filterCells[1].GetStatus();

        int orderById = filterCells[2].GetStatus();

        GameManager.Instance.apiHandler.ApplyAudtionCreateFilter(sortId, projectId, orderById, (status, response) => {

            if (status)
            {
                JoinedAuditionsResponse responseModel = JsonUtility.FromJson<JoinedAuditionsResponse>(response);

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
}
