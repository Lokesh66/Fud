using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PortfolioShareView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public GameObject shareButtonObject;

    public TMP_InputField searchField;

    public PortfolioShareTableView tableView;

    public PortfolioShareFilterView filterView;

    public NoDataView noDataView;


    [HideInInspector]
    public List<UserSearchModel> dataList;


    PortfolioSearchCell currentCell;

    PortfolioModel currentModel;

    UserSearchModel selectedModel = null;

    Action OnPortfolioShared;

    string keyword = string.Empty;

    string sourceFrom = "portfolio_share";

    bool isSearchAPICalled = false;


    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_USERS = 50;


    private void OnEnable()
    {
        keyword = string.Empty;

        searchField.text = keyword;
    }

    public void Load(PortfolioModel portfolioModel, Action OnPortfolioShared)
    {
        this.OnPortfolioShared = OnPortfolioShared;

        gameObject.SetActive(true);

        currentModel = portfolioModel;

        GameManager.Instance.apiHandler.GetAllUsers(pageNo, sourceFrom, (status, dataList) => {

            if (status)
            {
                this.dataList = dataList;

                if (dataList.Count < MAX_USERS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                if (!isInitialized)
                {
                    tableView.gameObject.SetActive(true);

                    isInitialized = true;
                }
                else
                {
                    tableView.Data.Clear();

                    tableView.Data.Add(dataList.Count);

                    tableView.Refresh();
                }
            }
        });
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

                currentCell.UpdateDeselectViw();

                if (string.IsNullOrEmpty(searchField.text))
                {
                    Load(currentModel, OnPortfolioShared);
                }
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

                Reload(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void Reload(List<UserSearchModel> searchModels)
    {
        dataList = searchModels;

        tableView.Data.Clear();

        tableView.Data.Add(searchModels.Count);

        tableView.Refresh();
    }

    public void OnSelectMember(PortfolioSearchCell selectedCell, object searchModel)
    {
        if (selectedCell != currentCell)
        {
            selectedModel = searchModel as UserSearchModel;

            searchField.text = selectedModel.name;

            currentCell?.UpdateDeselectViw();

            currentCell = selectedCell;

            shareButtonObject.SetActive(true);
        }
    }

    public void OnCellButtonAction(object searchModel)
    {
        selectedModel = searchModel as UserSearchModel;

        GameManager.Instance.apiHandler.GetUserInfo(selectedModel.id, (status, userInfo) => {

            if (status)
            {
                profileInfoView.Load(userInfo);
            }

        });
    }

    public void OnShareButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        int userId = selectedModel.id;

        GameManager.Instance.apiHandler.PostPortfolio(currentModel.id, userId, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Album Shared Successfully" : baseResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackAction();

        OnPortfolioShared?.Invoke();
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

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAllUsers(pageNo, sourceFrom, (status, modelsList) =>
        {
            if (status)
            {
                pageNo++;

                this.dataList = modelsList;

                if (modelsList.Count < MAX_USERS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                tableView.Data.Clear();

                tableView.Data.Add(modelsList.Count);

                tableView.Refresh();
            }
        });
    }

    void Reset()
    {
        //searchField.text = string.Empty;

        currentCell?.UpdateDeselectViw();

        currentCell = null;

        selectedModel = null;

        pageNo = 1;

        isPagingOver = false;

        shareButtonObject.SetActive(false);
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        dataList = data as List<UserSearchModel>;

        tableView.Data.Clear();

        tableView.Data.Add(dataList.Count);

        tableView.Refresh();

        noDataView.gameObject.SetActive(dataList.Count == 0);
    }
}
