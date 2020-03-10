using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SubscriptionView : MonoBehaviour
{
    public RectTransform content;

    public TMP_Dropdown dropdown;

    public GameObject subscriptionCell;


    List<Genre> genres;

    public void Load()
    {
        gameObject.SetActive(true);

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void OnValueChange()
    { 
    
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
