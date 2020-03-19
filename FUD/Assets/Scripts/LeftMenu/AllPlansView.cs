using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllPlansView : MonoBehaviour
{
    public RectTransform content;

    public TMP_Dropdown roleDropDown;

    public TMP_Dropdown durationDropDown;

    public GameObject planCell;


    List<SubscriptionModel> modelsList = null;

    List<Genre> genres;

    bool isDropDownEnabled = false;

    string selectedRole = string.Empty;

    string roleId = string.Empty;

    string selectedDuration = string.Empty;


    public void EnableView()
    {
        gameObject.SetActive(true);

        Load();

        ShowRoleDropDown();

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
        Debug.Log("All Plans Load Called");

        GameManager.Instance.apiHandler.GetSubscriptionPlans(roleId, GetDurationKey(durationDropDown.captionText.text), (status, response) => {

            if (status)
            {
                SubscriptionResponse responseModel = JsonUtility.FromJson<SubscriptionResponse>(response);

                modelsList = responseModel.data;

                SetView();
            }
        });
    }

    void SetView()
    {
        content.DestroyChildrens();

        for (int i = 0; i < modelsList.Count; i++)
        {
            GameObject planObject = Instantiate(planCell, content);

            planObject.GetComponent<SubscriptionPlanCell>().Load(modelsList[i], OnSubscriptionSelectAction);
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
}
