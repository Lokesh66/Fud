using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class AuditionOfferedFilterView : MonoBehaviour
{
    public TMP_Dropdown sortDropdown;

    public TMP_Dropdown orderDropdown;


    List<Genre> genres;

    Action<object> OnApplyFilter;


    public void Load(Action<object> OnApplyFilter)
    {
        gameObject.SetActive(true);

        this.OnApplyFilter = OnApplyFilter;
    }

    public void OnCancelButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnApplyButtonAction()
    {
        int sortId = sortDropdown.value;

        int orderById = orderDropdown.value + 1;


        GameManager.Instance.apiHandler.ApplyAudtionOfferedFilter(sortId, orderById, (status, response) => {

            if (status)
            {
                AuditionsResponse responseModel = JsonUtility.FromJson<AuditionsResponse>(response);

                OnApplyFilter?.Invoke(responseModel.data);

                OnCancelButtonAction();
            }   
        });
    }

    void ClearData()
    {
        orderDropdown.value = sortDropdown.value = 0;
    }
}
