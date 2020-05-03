using UnityEngine;
using TMPro;

public class SubscriptionController : MonoBehaviour
{
    public enum EScreenType
    { 
        None,
        AllPlans,
        Stores
    }

    public AllPlansView plansView;

    public StoresView storesView;

    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    private EScreenType currentTab = EScreenType.None;



    public void EnableView()
    {
        gameObject.SetActive(true);

        OnAllPlansTabAction();
    }

    public void OnAllPlansTabAction()
    {
        if (currentTab != EScreenType.AllPlans)
        {
            currentTab = EScreenType.AllPlans;

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

        currentTab = EScreenType.Stores;

        plansView.gameObject.SetActive(false);

        storesView.EnableView();
    }

    public void OnBackButtonAction()
    {
        if (currentTab == EScreenType.AllPlans)
        {
            plansView.ClearData();
        }
        else {
            storesView.ClearData();
        }

        currentTab = EScreenType.None;

        gameObject.SetActive(false);
    }
}
