using UnityEngine;
using TMPro;

public class SubscriptionController : MonoBehaviour
{
    public AllPlansView plansView;

    public AllPlansView storesView;

    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    private EStoreScreenType currentTab = EStoreScreenType.None;



    public void EnableView()
    {
        gameObject.SetActive(true);

        OnAllPlansTabAction();
    }

    public void OnAllPlansTabAction()
    {
        if (currentTab != EStoreScreenType.AllPlans)
        {
            currentTab = EStoreScreenType.AllPlans;

            storesView.gameObject.SetActive(false);

            plansView.EnableView();

            buttonsList[0].color = selectedColor;

            buttonsList[1].color = disabledColor;
        }
    }

    public void OnStoresTabAction()
    {
        buttonsList[0].color = disabledColor;

        buttonsList[1].color = selectedColor;

        currentTab = EStoreScreenType.Stores;

        plansView.gameObject.SetActive(false);

        storesView.EnableView();
    }

    public void OnBackButtonAction()
    {
        if (currentTab == EStoreScreenType.AllPlans)
        {
            plansView.ClearData();
        }
        else {
            storesView.ClearData();
        }

        currentTab = EStoreScreenType.None;

        gameObject.SetActive(false);
    }
}

public enum EStoreScreenType
{    
    AllPlans = 0,
    Stores,
    None,
}
