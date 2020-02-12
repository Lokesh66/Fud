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

    public PortfolioBasicInfoView basicInfoView;

    public PortfolioMediaView mediaView;

    public PortfolioExperianceView experianceView;

    public GameObject createPortfolioCache;

    public RectTransform parentTrans;

    public Color selectedColor;

    public Color disabledColor;


    public GameObject createWorkExperianceCache;


    private GameObject currentObject;

    private ETabType currentTab;


    protected override void EnableView()
    {
        base.EnableView();

        UpdateScreen();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();

        gameObject.SetActive(true);

        UpdateScreen();
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

    public void OnCreateAction()
    {
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
        currentObject = basicInfoView.gameObject;

        basicInfoView.SetView();
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

        OnAddSubView(creationObject);

        gameObject.SetActive(false);

        creationObject.GetComponent<UpdateExperianceView>().Load(this);
    }
}
