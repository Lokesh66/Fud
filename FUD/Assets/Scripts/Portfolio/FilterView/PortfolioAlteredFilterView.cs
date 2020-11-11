using System.Collections.Generic;
using UnityEngine;
using System;


public class PortfolioAlteredFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;

    public FilterCell categeryFilterCell;


    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        filterCells[2].OnRoleSelection(OnRoleSelectAction);
    }

    void OnRoleSelectAction(object selectedRole)
    {
        CraftRoleModel roleModel = selectedRole as CraftRoleModel;

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

                filterCells[2].Load(dropdownModels);
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
        int statusId = filterCells[0].GetStatus();

        int genreId = filterCells[1].GetStatus();

        int roleCategeryId = filterCells[2].GetStatus();

        int sortId = filterCells[3].GetStatus();

        int orderById = filterCells[4].GetStatus();

        GameManager.Instance.apiHandler.ApplyPortfolioAlteredFilter(genreId, roleCategeryId, statusId, sortId, orderById, (status, resopnse) => {

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
        for (int i = 0; i < filterCells.Count; i++)
        {
            filterCells[i].ClearSelectedModels();
        }
    }
}
