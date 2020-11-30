using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class StoryHistoryFilterView : MonoBehaviour
{
    public List<FilterCell> filterCells;


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
        int sortId = filterCells[0].GetStatus();

        GameManager.Instance.apiHandler.ApplyStoryHistoryFilter(filterCells[0].filterKeys[sortId], (status, resopnse) =>
        {
            Debug.Log("Response = " + resopnse);

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
}
