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

    public TextMeshProUGUI[] buttonList;

    public PortfolioBasicInfo basicInfoView;

    public PortfolioMediaView mediaView;

    public PortfolioExperianceView experianceView;

    public ProfileInfoView infoView;

    public GameObject createPortfolioCache;

    public GameObject createPanel;

    public RectTransform parentTrans;

    public Color selectedColor;

    public Color disabledColor;


    public GameObject createWorkExperianceCache;


    private GameObject currentObject;

    private ETabType currentTab;


    protected override void EnableView()
    {
        base.EnableView();

        ShowMedia();

        ShowExperianceScreen();

        ShowBasicInfo();

        //UpdateScreen();
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


    public void OnTabAction(int tabIndex)
    {
        ETabType clickedTab = (ETabType)tabIndex;

        if (currentTab != clickedTab)
        {
            buttonList[(int)currentTab].color = disabledColor;

            currentTab = clickedTab;

            currentObject?.SetActive(false);
        }

        UpdateScreen();
    }

    void OnCreateAction()
    {
        createPanel.SetActive(false);

        ShowCreatePortfolioScreen();
    }

    void UpdateScreen()
    {
        buttonList[(int)currentTab].color = selectedColor;

        switch (currentTab)
        {
            case ETabType.BasicInfo:
                ShowBasicInfo();
                break;
            case ETabType.Media:
                ShowMedia();
                break;
            case ETabType.Experiance:
                ShowExperianceScreen();
                break;
        }

        currentObject.SetActive(true);
    }

    void ShowBasicInfo()
    {
        basicInfoView.Load();
    }

    void ShowMedia()
    {
        currentObject = mediaView.gameObject;

        mediaView.Load();
    }

    void ShowExperianceScreen()
    {
        currentObject = experianceView.gameObject;

        experianceView.Load(this);
    }
 

    void ShowCreatePortfolioScreen()
    {
        GameObject creationObject = Instantiate(createPortfolioCache, parentTrans);

        OnAddSubView(creationObject);

        gameObject.SetActive(false);

        creationObject.GetComponent<PortfolioCreationView>().Init(this);
    }

    public void OnExperianceCreateAction()
    {
        GameObject creationObject = Instantiate(createWorkExperianceCache, parentTrans);

        //OnAddSubView(creationObject);

        createPanel.SetActive(false);

        creationObject.GetComponent<CreateExperienceView>().Load(this);
    }

    public void OnEditButtonAction()
    {
        infoView.Load(OnPortifolioClose);     
    }

    void OnPortifolioClose(bool isDataUpdated)
    {
        if (isDataUpdated)
        {
            ShowBasicInfo();
        }
    }

    public void OnCreateButtonAction()
    {
        createPanel.SetActive(true);

        //infoView.gameObject.SetActive(true);
    }

    public void OnDismissCreation()
    {
        createPanel.SetActive(false);
    }
}
