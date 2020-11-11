using UnityEngine;
using TMPro;


public class PortfolioView : BaseView
{
    public TextMeshProUGUI[] buttonList;

    public RectTransform parentTrans;

    public GameObject createPanel;

    public GameObject createButtonObject;

    public Color selectedColor;

    public Color disabledColor;


    public PortfolioHandler portfolioHandler;
    
    public PortfolioOfferedView offeredView;

    public PortfolioAlteredView alteredView;


    public GameObject createPortfolioCache;

    public GameObject createWorkExperianceCache;


    public GameObject filterObject;


    private ETabType currentTab = ETabType.Offers;

    private GameObject currentObject;


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

        portfolioHandler.Load(this);
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();

        ResetScreen();
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ETabType)tabIndex)
        {
            buttonList[(int)currentTab].color = disabledColor;

            buttonList[tabIndex].color = selectedColor;

            currentTab = (ETabType)tabIndex;

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        createButtonObject.SetActive(currentTab == ETabType.Created);

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
                currentObject = portfolioHandler.gameObject;
                portfolioHandler.Load(this);
                break;
        }
    }

    public void OnCreateButtonAction()
    {
        createPanel.SetActive(true);
    }

    public void ShowCreatePortfolioScreen()
    {
        if (DataManager.Instance.CanLoadScreen(EFeatureType.PortfolioCreation))
        {
            GameObject creationObject = Instantiate(createPortfolioCache, parentTrans);

            gameObject.SetActive(false);

            createPanel.SetActive(false);

            creationObject.GetComponent<PortfolioCreationView>().Init(this);
        }
        else
        {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.PortfolioCreation);
        }
    }

    public void OnExperianceCreateAction()
    {
        GameObject creationObject = Instantiate(createWorkExperianceCache, parentTrans);

        createPanel.SetActive(false);

        creationObject.GetComponent<CreateExperienceView>().Load(this);
    }

    public void OnDismissCreation()
    {
        createPanel.SetActive(false);
    }

    void ResetScreen()
    {
        buttonList[(int)currentTab].color = disabledColor;

        buttonList[0].color = selectedColor;

        currentTab = ETabType.Offers;

        currentObject?.SetActive(false);

        currentObject = null;
    }

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

    public void EnableCurrentScreen()
    {
        gameObject.SetActive(true);

        UpdateCurrentView();
    }
}
