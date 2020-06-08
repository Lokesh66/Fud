using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AllPlansView : MonoBehaviour
{
    public RectTransform content;

    public ScrollRect scrollRect;

    public TMP_Dropdown roleDropDown;

    public TMP_Dropdown durationDropDown;

    public GameObject planCell;

    public GameObject noDataObject;

    public EStoreScreenType screenType;


    private List<Canvas> scrollCanvases = new List<Canvas>();

    List<SubscriptionModel> modelsList = null;

    List<Genre> genres;

    string selectedRole = string.Empty;

    string roleId = string.Empty;

    string selectedDuration = string.Empty;


    public void EnableView()
    {
        gameObject.SetActive(true);

        durationDropDown.value = 0;

        selectedDuration = durationDropDown.captionText.text;

        ShowRoleDropDown();

        roleDropDown.value = 0;

        OnRoleValueChange();

        durationDropDown.onValueChanged.RemoveAllListeners();

        durationDropDown.onValueChanged.AddListener(delegate {
            OnDurationValueChange();
        });
    }

    void OnRoleValueChange()
    {
        if (!selectedRole.Equals(roleDropDown.captionText.text))
        {
            selectedRole = roleDropDown.captionText.text;

            Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedRole));

            roleId = selectedGenre.id.ToString();

            Load();
        }
    }

    void OnDurationValueChange()
    {
        if (!selectedDuration.Equals(durationDropDown.captionText.text))
        {
            selectedDuration = durationDropDown.captionText.text;

            Load();
        }
    }

    void Load()
    {
        GameManager.Instance.apiHandler.GetSubscriptionPlans((int)screenType, roleId, GetDurationKey(selectedDuration), (status, response) => {

            if (status)
            {
                SubscriptionResponse responseModel = JsonUtility.FromJson<SubscriptionResponse>(response);

                modelsList = responseModel.data;

                noDataObject.SetActive(modelsList.Count == 0);

                if (screenType == EStoreScreenType.AllPlans)
                {
                    SetAllPlansView();
                }
                else {
                    SetStoreView();
                }
            }
        });
    }

    void SetAllPlansView()
    {
        content.DestroyChildrens();

        for (int i = 0; i < modelsList.Count; i++)
        {
            GameObject planObject = Instantiate(planCell, content);

            modelsList[i].SetPlanPrice(selectedDuration);

            SubscriptionPlanCell subscriptionPlanCell = planObject.GetComponent<SubscriptionPlanCell>();

            scrollCanvases.Add(subscriptionPlanCell.scrollCanvas);

            subscriptionPlanCell.Load(modelsList[i], OnSubscriptionSelectAction);
        }
    }

    void SetStoreView()
    {
        content.DestroyChildrens();

        for (int i = 0; i < modelsList.Count; i++)
        {
            GameObject planObject = Instantiate(planCell, content);

            modelsList[i].SetPlanPrice(selectedDuration);

            StoreCell storeCell = planObject.GetComponent<StoreCell>();

            storeCell.Load(modelsList[i], OnSubscriptionSelectAction);
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
            DataManager.Instance.userInfo.email_id, planIdInfo);//, OnResponse: OnResponse);
    }

    void OnResponse(bool status, string orderId)
    {
        if (status)
        {
            GameManager.Instance.apiHandler.VerifyPurchsedOrderId(orderId, (apiStatus) =>
            {
                if (apiStatus)
                {
                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "Order Verified Success";

                    UIManager.Instance.ShowAlert(alertModel);
                }
                else
                {
                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "Order verification Failed";

                    UIManager.Instance.ShowAlert(alertModel);
                }
            });
        }
    }

    void ShowRoleDropDown()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();
        roleDropDown.AddOptions(options);

        roleDropDown.onValueChanged.RemoveAllListeners();

        roleDropDown.onValueChanged.AddListener(delegate {
            OnRoleValueChange();
        });
    }

    string GetDurationKey(string text)
    {
        string key = string.Empty;

        switch (text)
        {
            case "Monthly":
                key = "price_per_month";
                break;

            case "Yearly":
                key = "price_per_year";
                break;

            case "Forever":
                key = "price_per_forever";
                break;
        }

        return key;
    }

    public void ClearData()
    {
        roleId = selectedDuration = selectedRole = string.Empty;

        durationDropDown.onValueChanged.RemoveAllListeners();

        roleDropDown.onValueChanged.RemoveAllListeners();
    }

    public void OnDrag()
    {
        foreach (var item in scrollCanvases)
        {
            item.overrideSorting = false;
        }
    }

    public void OnEndDrag()
    {
        foreach (var item in scrollCanvases)
        {
            item.overrideSorting = true;
        }
    }
}
