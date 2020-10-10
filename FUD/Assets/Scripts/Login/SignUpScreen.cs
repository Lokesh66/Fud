using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class SignUpScreen : MonoBehaviour
{
    public RectTransform rolesContent;

    public GameObject roleObject;

    public GameObject roleScrollObject;

    public TermsAndConditionsView termsView;

    public TMP_InputField nameFieldText;
    public TMP_InputField numberFieldText;
    public TMP_InputField roleField;

    public TMP_Dropdown countryDropdown;

    public Toggle termsToggle;

    System.Action<bool, object> OnButtonAction;

    List<CountryModel> countryModels;

    List<Craft> craftsList;

    Craft selectedModel;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;


    public void SetView(System.Action<bool, object> action)
    {
        gameObject.SetActive(true);

        OnButtonAction = action;

        SetCountryDropdown();

        craftsList = DataManager.Instance.crafts;
    }

    void SetRoleSelectionScroll(List<Craft> craftsList)
    {
        rolesContent.DestroyChildrens();

        roleScrollObject.SetActive(craftsList.Count > 0);


        for (int i = 0; i < craftsList.Count; i++)
        {
            CraftRoleItem item = Instantiate(roleObject, rolesContent).GetComponent<CraftRoleItem>();

            item.SetView(craftsList[i], OnSelectMember);
        }
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as Craft;

        roleField.text = selectedModel.name;

        rolesContent.DestroyChildrens();

        roleScrollObject.SetActive(false);
    }

    void SetCountryDropdown()
    {
        GameManager.Instance.apiHandler.GetCountries((status, modelsList) => {

            if (status)
            {
                countryModels = modelsList;

                List<string> options = new List<string>();

                foreach (var option in countryModels)
                {
                    options.Add(option.name);
                }

                countryDropdown.ClearOptions();

                countryDropdown.AddOptions(options);
            }
        });
    }

    void GetSearchedRoles()
    {
        List<Craft> searchedCrafts = craftsList.FindAll(item => item.name.ToLower().Contains(keyword.ToLower()) || keyword.ToLower().Contains(item.name.ToLower()));

        SetRoleSelectionScroll(searchedCrafts);

        isSearchAPICalled = false;
    }

    public void OnRoleFieldSelect()
    {
        if (!roleField.text.IsNOTNullOrEmpty())
        {
            SetRoleSelectionScroll(craftsList);
        }
    }

    public void OnRoleValueChange()
    {
        if (selectedModel == null)
        {
            if (roleField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = roleField.text;

                GetSearchedRoles();
            }
            else
            {
                if (!roleField.text.IsNOTNullOrEmpty())
                {
                    SetRoleSelectionScroll(craftsList);
                }
                else
                {
                    rolesContent.DestroyChildrens();

                    roleScrollObject.SetActive(false);
                }
            }
        }
        else
        {
            if (!roleField.text.Equals(selectedModel.name))
            {
                selectedModel = null;

                OnRoleValueChange();
            }
        }
    }

    public void OnClick_SendOTP()
    {
        if (string.IsNullOrEmpty(numberFieldText.text))
        {
            return;
        }
        else if (!termsToggle.isOn)
        {
            ShowValidationMessage("Please accept terms and conditions to continue");
        }

        Dictionary<string, object> body = new Dictionary<string, object>();

        CountryModel selectedModel = countryModels[countryDropdown.value];

        body.Add("name", nameFieldText.text);

        body.Add("number", numberFieldText.text);

        body.Add("country_id", selectedModel.id);

        body.Add("role_id", this.selectedModel.id);

        body.Add("agree_terms_condition", 1);

        OnButtonAction?.Invoke(true, body);

        ClearData();
    }

    public void OnClick_Back()
    {
        OnButtonAction?.Invoke(false, 0);

        ClearData();
    }

    public void OnTermsAndConditionsButtonAction()
    {
        termsView.Load();
    }

    void ClearData()
    {
        termsToggle.isOn = false;

        rolesContent.DestroyChildrens();

        roleScrollObject.SetActive(false);

        roleField.text = keyword = nameFieldText.text = numberFieldText.text = string.Empty;

        selectedModel = null;

        countryDropdown.value = 0;
    }

    void ShowValidationMessage(string message)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = message;

        UIManager.Instance.ShowAlert(alertModel);
    }
}
