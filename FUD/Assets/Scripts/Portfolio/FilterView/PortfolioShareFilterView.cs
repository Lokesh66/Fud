using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class PortfolioShareFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;

    public FilterCell categeryFilterCell;

    public FilterCell roleFilterCell;

    public TMP_InputField nameField;


    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;

        SetIndustriesView();

        filterCells[2].OnRoleSelection(OnRoleSelectAction);

        filterCells[0].OnCastCategeryChange(OnCastCategeryChange);
    }

    void OnRoleSelectAction(object selectedRole)
    {
        Craft roleModel = selectedRole as Craft;

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

    void OnCastCategeryChange(int isCast)
    {
        List<Craft> modelsList = isCast == 0 ? DataManager.Instance.GetCraftRolesOnCategery(true) : DataManager.Instance.GetCraftRolesOnCategery(false);

        DropdownModel dropdownModel = null;

        List<DropdownModel> dropdownModels = new List<DropdownModel>();

        for (int i = 0; i < modelsList.Count; i++)
        {
            dropdownModel = new DropdownModel();

            dropdownModel.text = modelsList[i].name;

            dropdownModel.id = modelsList[i].id;

            dropdownModels.Add(dropdownModel);
        }

        filterCells[1].Load(dropdownModels);

        roleFilterCell.gameObject.SetActive(true);
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        roleFilterCell.gameObject.SetActive(false);

        categeryFilterCell.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int roleId = filterCells[0].GetStatus();

        int roleCategeryId = filterCells[1].GetStatus();

        int userType = filterCells[2].GetStatus();

        int industryId = filterCells[3].GetStatus();


        GameManager.Instance.apiHandler.ApplyShareStoryFilter(nameField.text, roleId, roleCategeryId, userType, industryId,(status, resopnse) =>
        {
            if (status)
            {
                OnApplyFilter?.Invoke(resopnse);

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

    void SetIndustriesView()
    {
        List<IndustryModel> industryModels;

        DataManager.Instance.GetIndustries((modelsList) => {

            industryModels = modelsList;

            DropdownModel dropdownModel = null;

            List<DropdownModel> dropdownModels = new List<DropdownModel>();

            for (int i = 0; i < industryModels.Count; i++)
            {
                dropdownModel = new DropdownModel();

                dropdownModel.text = industryModels[i].name;

                dropdownModel.id = industryModels[i].id;

                dropdownModels.Add(dropdownModel);
            }

            filterCells[4].Load(dropdownModels);
        });
    }

    string GetGenderType(int index)
    {
        string gender = string.Empty;

        switch(index)
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
