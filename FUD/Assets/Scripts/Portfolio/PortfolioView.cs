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
        Portfolio,
        Activities
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


    private EPortfolioTab currentTab;


    protected override void EnableView()
    {
        base.EnableView();

        OnPortfolioButtonAction();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();

        gameObject.SetActive(true);

        //UpdateScreen();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();
    }

    #region Button Actions

    public void OnPortfolioButtonAction()
    {
        currentTab = EPortfolioTab.Portfolio;

        activitiesView.gameObject.SetActive(false);

        portfolioHandler.Load();

        buttonList[0].color = selectedColor;

        buttonList[1].color = disabledColor;
    }

    public void OnActivitiesTabAction()
    {
        buttonList[0].color = disabledColor;

        buttonList[1].color = selectedColor;

        currentTab = EPortfolioTab.Activities;

        portfolioHandler.gameObject.SetActive(false);

        ShowActivitiesScreen();
    }

    #endregion

    void ShowActivitiesScreen()
    {
        activitiesView.EnableView();
    }

    public void OnCreateButtonAction()
    {
        createPanel.SetActive(true);

        //infoView.gameObject.SetActive(true);
    }

    public void ShowCreatePortfolioScreen()
    {
        GameObject creationObject = Instantiate(createPortfolioCache, parentTrans);

        gameObject.SetActive(false);

        createPanel.SetActive(false);

        creationObject.GetComponent<PortfolioCreationView>().Init(this);
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
}
