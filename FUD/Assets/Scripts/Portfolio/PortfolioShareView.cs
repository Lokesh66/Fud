using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortfolioShareView : MonoBehaviour
{
    public RectTransform searchContent;

    public GameObject searchCell;

    public TMP_InputField searchField;

    //public TMP_InputField commentField;


    PortfolioModel currentModel;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;


    private void OnEnable()
    {
        keyword = string.Empty;

        searchField.text = keyword;
    }

    public void Load(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        currentModel = portfolioModel;
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            if (searchField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = searchField.text;

                GetSearchedUsers();
            }
        }
        else
        {
            if (!searchField.text.Equals(selectedModel.name))
            {
                selectedModel = null;
            }
        }
    }

    void GetSearchedUsers()
    {
        GameManager.Instance.apiHandler.SearchTeamMember(keyword, (status, response) =>
        {
            if (status)
            {
                UserSearchResponse searchResponse = JsonUtility.FromJson<UserSearchResponse>(response);

                PopulateDropdown(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        for (int i = 0; i < searchModels.Count; i++)
        {
            cellObject = Instantiate(searchCell, searchContent);

            cellObject.GetComponent<PortfolioSearchCell>().SetView(searchModels[i], OnSelectMember);
        }
    }

    void OnSelectMember(object searchModel)
    {
        selectedModel = searchModel as UserSearchModel;

        searchField.text = selectedModel.name;

        searchContent.DestroyChildrens();
    }

    
    public void OnShareButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        int userId = selectedModel.id;

        GameManager.Instance.apiHandler.PostPortfolio(currentModel.id, userId, (status, response) => {

            if (status)
            {

            }

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Album Shared Successfully" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        OnBackAction();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(searchField.text))
        {
            errorMessage = "Please add members to share.";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return false;
        }

        return true;
    }

    public void OnBackAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void Reset()
    {
        searchField.text = string.Empty;

        searchContent.DestroyChildrens();
    }
}
