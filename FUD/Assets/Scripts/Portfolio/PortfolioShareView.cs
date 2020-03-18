using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortfolioShareView : MonoBehaviour
{
    public RectTransform searchContent;

    public GameObject searchCell;

    public TMP_InputField searchField;

    public TMP_InputField commentField;


    PortfolioModel currentModel;

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
        if (searchField.text.Length > 2 && !isSearchAPICalled)
        {
            //Call Search API
            isSearchAPICalled = true;

            keyword = searchField.text;

            GetSearchedUsers();
        }
        else {
            if (searchField.text.Length == 0)
            {
                searchContent.DestroyChildrens();
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

    void OnSelectMember(object id)
    {
        if (!CanCallAPI())
        {
            return;
        }

        int userId = (int)id;

        GameManager.Instance.apiHandler.PostPortfolio(currentModel.id, commentField.text, userId, (status, response) => {

            if (status)
            {

            }

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Creation Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        CanvasManager.Instance.alertView.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        OnBackAction();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(commentField.text))
        {
            errorMessage = "Please add the comment";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            CanvasManager.Instance.alertView.ShowAlert(alertModel);
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
        searchField.text = commentField.text = string.Empty;

        searchContent.DestroyChildrens();
    }
}
