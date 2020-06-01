using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PortfolioView : BaseView
{
    public enum ETabType
    { 
        BasicInfo,
        Media,
        Experiance
    }

    public enum EPortfolioTab
    {
        Offers,
        Altered,
        Portfolio,
    }

    public TextMeshProUGUI[] buttonList;

    public RectTransform parentTrans;

    public GameObject createPanel;

    public Color selectedColor;

    public Color disabledColor;


    public PortfolioHandler portfolioHandler;
    
    public PortfolioActivitiesView activitiesView;


    public GameObject createPortfolioCache;

    public GameObject createWorkExperianceCache;


    private EPortfolioTab currentTab = EPortfolioTab.Offers;

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

        gameObject.SetActive(true);

        portfolioHandler.Load();

        //UpdateScreen();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();

        ResetScreen();
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (EPortfolioTab)tabIndex)
        {
            buttonList[(int)currentTab].color = disabledColor;

            buttonList[tabIndex].color = selectedColor;

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case EPortfolioTab.Offers:
                currentObject = activitiesView.gameObject;
                activitiesView.EnableView();
                break;

            case EPortfolioTab.Altered:
                //currentObject = alteredView.gameObject;
                //alteredView.EnableView(this);
                break;

            case EPortfolioTab.Portfolio:
                currentObject = portfolioHandler.gameObject;
                portfolioHandler.Load();
                break;
        }
    }

    public void OnCreateButtonAction()
    {
        createPanel.SetActive(true);

        //infoView.gameObject.SetActive(true);
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

        //OnAddSubView(creationObject);

        createPanel.SetActive(false);

        creationObject.GetComponent<CreateExperienceView>().Load(this);
    }

    public void OnDismissCreation()
    {
        createPanel.SetActive(false);
    }

    void ResetScreen()
    {
        portfolioHandler.infoView.OnBackButtonAction();
    }
}
