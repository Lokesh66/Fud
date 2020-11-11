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


    private List<Canvas> scrollCanvases = new List<Canvas>();

    List<SubscriptionModel> modelsList = null;

    List<Craft> craftRoles;

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

            Craft selectedCraft = craftRoles.Find(genre => genre.name.Equals(selectedRole));

            roleId = selectedCraft.id.ToString();

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
        GameManager.Instance.apiHandler.GetSubscriptionPlans(0, roleId, GetDurationKey(selectedDuration), (status, response) => {

            if (status)
            {
                SubscriptionResponse responseModel = JsonUtility.FromJson<SubscriptionResponse>(response);

                modelsList = responseModel.data;

                noDataObject.SetActive(modelsList.Count == 0);

                SetAllPlansView();
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

    void OnSubscriptionSelectAction(SubscriptionModel model)
    {
        Dictionary<string, object> planIdInfo = new Dictionary<string, object>
        {
            { "plan_id", model.id }
        };
        GameManager.Instance.apiHandler.CashFreeRequest(DataManager.Instance.userInfo.phone.ToString(),
            DataManager.Instance.userInfo.email_id, planIdInfo);
    }

    void ShowRoleDropDown()
    {
        craftRoles = DataManager.Instance.crafts;

        List<string> options = new List<string>();

        foreach (var option in craftRoles)
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
