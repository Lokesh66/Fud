using UnityEngine;
using TMPro;


public class ProfileView : MonoBehaviour
{
    public enum EProfileTabType
    {
        Info,
        Features
    }

    public MyProfilePanel profileView;

    public ProfileFeaturesView featuresView;


    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    private GameObject currentObject;

    private EProfileTabType currentTab = EProfileTabType.Features;


    public void Load()
    {
        gameObject.SetActive(true);

        OnTabAction(0);
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (EProfileTabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (EProfileTabType)tabIndex;

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    public void OnBackButtonAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    #endregion

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case EProfileTabType.Info:
                currentObject = profileView.gameObject;
                profileView.Load();
                break;

            case EProfileTabType.Features:
                currentObject = featuresView.gameObject;
                featuresView.Load(OnCloseFeaturesView);
                break;
        }
    }

    void OnCloseFeaturesView(bool isGoingToStore)
    {
        if(isGoingToStore)
        {
            OnBackButtonAction();
        }
    }

    void ClearData()
    {
        currentTab = EProfileTabType.Features;

        currentObject?.SetActive(false);

        currentObject = null;
    }
}
