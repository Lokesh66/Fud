using UnityEngine;
using TMPro;


public class AuditionView : BaseView
{
    public TextMeshProUGUI[] buttonsList;

    public AuditionOfferedView offeredView;

    public AuditionAlteredView alteredView;

    public AuditionCreatedView createdView;

    public GameObject addObject;

    public GameObject filterObject;


    public Color selectedColor;

    public Color disabledColor;


    private GameObject currentObject;

    private ETabType currentTab = ETabType.Offers;


    protected override void EnableView()
    {
        base.EnableView();

        UpdateCurrentView();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ETabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (ETabType)tabIndex;

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    void UpdateCurrentView()
    {
        addObject.SetActive(currentTab == ETabType.Created);

        filterObject.SetActive(currentTab != ETabType.Created);

        switch (currentTab)
        {
            case ETabType.Offers:
                currentObject = offeredView.gameObject;
                offeredView.Load();
                break;

            case ETabType.Altered:
                currentObject = alteredView.gameObject;
                alteredView.Load();
                break;

            case ETabType.Created:
                currentObject = createdView.gameObject;
                createdView.Load();
                break;
        }
    }

    public void OnAddButtonAction()
    {
        if (DataManager.Instance.CanLoadScreen(EFeatureType.AuditionCreation))
        {
            CreateAuditionView.Instance.SetView(1, (isNewDataUpdated) => {

                if (isNewDataUpdated)
                {

                }
            });
        }
        else
        {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.AuditionCreation);
        }
    }

    #endregion

    public void OnFilterButtonAction()
    {
        if (currentTab == ETabType.Offers)
        {
            offeredView.OnFilterButtonAction();
        }
        else
        {
            alteredView.OnFilterButtonAction();
        }
    }
}
