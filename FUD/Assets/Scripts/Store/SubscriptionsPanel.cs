using System.Collections.Generic;
using UnityEngine;

public class SubscriptionsPanel : MonoBehaviour
{
    public GameObject subscriptionCell;
    public Transform parentContent;
    List<SubscriptionModel> subscriptionsList = new List<SubscriptionModel>();

    public void Load()
    {
        gameObject.SetActive(true);
        GetSubscriptionPlans();
    }

    void GetSubscriptionPlans()
    {
        subscriptionsList = new List<SubscriptionModel>();
        string roleId = DataManager.Instance.userInfo.role_id.ToString();
        roleId = "";
        GameManager.Instance.apiHandler.GetSubscriptionPlans(roleId, (status, response) => {

            if (status)
            {
                SubscriptionResponse subscriptionResponse = JsonUtility.FromJson<SubscriptionResponse>(response);
                this.subscriptionsList = subscriptionResponse.data;
                    
                SetView();
            }
        });
    }

    void SetView()
    {
        parentContent.DestroyChildrens();

        if (subscriptionsList == null)
        {
            subscriptionsList = new List<SubscriptionModel>();
        }

        Debug.Log("SetView : "+subscriptionsList.Count);

        for (int i = 0; i < subscriptionsList.Count; i++)
        {
            GameObject go = Instantiate(subscriptionCell, parentContent);

            SubscriptionCell cell = go.GetComponent<SubscriptionCell>();

            cell.SetView(subscriptionsList[i], OnSubscriptionSelectAction);
        }
    }

    void OnSubscriptionSelectAction(SubscriptionModel model)
    {
        Debug.Log("OnSubscriptionSelectAction : " + model.name);

        Dictionary<string, object> planIdInfo = new Dictionary<string, object>
        {
            { "plan_id", model.id }
        };
        Debug.Log("CashFree button action");
        GameManager.Instance.apiHandler.CashFreeRequest(DataManager.Instance.userInfo.phone.ToString(),
            DataManager.Instance.userInfo.email_id, planIdInfo);
    }

    public void BackAction()
    {
        gameObject.SetActive(false);
    }
}
