using UnityEngine;


public class SubscriptionController : MonoBehaviour
{
    public AllPlansView plansView;


    public void EnableView()
    {
        gameObject.SetActive(true);

        plansView.EnableView();
    }

    public void OnBackButtonAction()
    {
        plansView.ClearData();

        gameObject.SetActive(false);
    }
}
