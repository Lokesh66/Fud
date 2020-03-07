using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryDetailsController : MonoBehaviour
{
    #region Singleton

    private static StoryDetailsController instance = null;
    private StoryDetailsController()
    {

    }

    public static StoryDetailsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StoryDetailsController>();
            }
            return instance;
        }
    }

    #endregion
    public enum EScreenSubType
    {
        Details,
        Versions,
        Characters,
        Team,
    }

    public Transform creationPanelParent;

    public StoryDetailsView storyDetailsView;

    public StoryVersionsView versionsView;

    public StoryCharactersView charactersView;

    public StoryTeamView teamsView;

    public TextMeshProUGUI titeText;

    public TextMeshProUGUI[] buttonList;

    public GameObject storyVersionObject;

    public GameObject addCharacterCache;

    public GameObject addTeamMemberCache;

    public GameObject updateStoryCache;


    public GameObject noDataObject;


    public Color selectedColor;

    public Color disabledColor;


    private GameObject currentObject;

    private EScreenSubType currentType;

    private GameObject currentCreateScreen;

    StoryDetailsModel currentDetailsModel;

    Stack<string> detailResponses = new Stack<string>();


    /*MyStoriesController storiesController;*/

    System.Action BackAction;

    public void Load(object storyId, System.Action onBackAction)
    {        
        GameManager.Instance.apiHandler.GetStoryDetails((int)storyId, (status, response) =>
        {
            if (status)
            {
                StoryDetailsResponseModel responseModel = JsonUtility.FromJson<StoryDetailsResponseModel>(response);

                if (responseModel.data != null && responseModel.data.Count > 0)
                {
                    creationPanelParent.gameObject.SetActive(true);

                    BackAction = onBackAction;

                    currentDetailsModel = responseModel.data[0];

                    UpdateScreen();
                }
                else
                {
                    onBackAction?.Invoke();
                }
            }
            else
            {
                onBackAction?.Invoke();
            }
        });


    }

    public void EnableView(StoryDetailsModel detailsModel)
    {
        currentDetailsModel = detailsModel;
    }

    public void OnBackButtonAction()
    {
        creationPanelParent.gameObject.SetActive(false);

        Destroy(currentCreateScreen);

        BackAction?.Invoke();

        ResetData();

        BackAction = null;
    }

    public void OnTabAction(int tabIndex)
    {
        UpdateScreen(tabIndex);
    }

    void UpdateScreen(int tabIndex)
    {
        EScreenSubType screenSubType = (EScreenSubType)tabIndex;

        if (currentType != screenSubType)
        {
            buttonList[(int)currentType].color = disabledColor;

            noDataObject.SetActive(false);

            currentType = screenSubType;

            titeText.text = currentDetailsModel.title;

            currentObject?.SetActive(false);

            UpdateScreen();
        }
    }

    public void UpdateScreen()
    {
        buttonList[(int)currentType].color = selectedColor;

        DestroyCreationScreen();        

        switch (currentType)
        {
            case EScreenSubType.Details:
                GetStoryDetails();
                break;
            case EScreenSubType.Versions:
                GetStoryVersions();
                break;
            case EScreenSubType.Characters:
                ShowCharactersScreen();
                break;
            case EScreenSubType.Team:
                ShowTeamsScreen();
                break;
        }
    }

    public string GetStoryTitle()
    {
        return currentDetailsModel.title;
    }

    public int GetStoryId()
    {
        return currentDetailsModel.id;
    }

    void DestroyCreationScreen()
    {
        Destroy(currentCreateScreen);
    }

    #region Tabs Screens Info

    void GetStoryDetails()
    {
        Debug.Log("GetStoryDetails Called");
        currentDetailsModel.SetDetailsScreenModel();

        currentObject = storyDetailsView.gameObject;

        storyDetailsView.SetView(currentDetailsModel.screenModel);
    }

    void GetStoryVersions()
    {
        currentObject = versionsView.gameObject;

        versionsView.EnableView(currentDetailsModel.StoryVersions);
    }

    void ShowCharactersScreen()
    {
        currentObject = charactersView.gameObject;

        currentObject.SetActive(false);

        charactersView.Load(currentDetailsModel.StoryCharacters, this);
    }

    void ShowTeamsScreen()
    {
        currentObject = teamsView.gameObject;

        currentObject.SetActive(false);

        teamsView.Load(currentDetailsModel.TeamMembers, this);
    }

    #endregion

    public void OnAddButtonAction()
    {
        noDataObject.SetActive(false);

        switch (currentType)
        {
            case EScreenSubType.Versions:
                CreateStoryVersion();
                break;
            case EScreenSubType.Characters:
                CreateCharacter();
                break;
            case EScreenSubType.Team:
                AddTeamMember();
                break;
        }
    }

    public void OnUpdateStory()
    {
        
    }

    void CreateStoryVersion()
    {
        currentObject?.SetActive(false);

        //currentCreateScreen = storyVersionObject;

        storyVersionObject.GetComponent<CreateStoryVersion>().Load(versionsView);
    }

    void CreateCharacter()
    {
        currentObject?.SetActive(false);

        GameObject addCharacterObject = Instantiate(addCharacterCache, creationPanelParent);

        currentCreateScreen = addCharacterObject;

        addCharacterObject.GetComponent<CreateCharacterView>().SetView(currentDetailsModel, UpdataCharacters);
    }

    void AddTeamMember()
    {
        currentObject?.SetActive(false);

        GameObject addTeamObject = Instantiate(addTeamMemberCache, creationPanelParent);

        currentCreateScreen = addTeamObject;

        addTeamObject.GetComponent<CreateTeamView>().SetView(currentDetailsModel, UpdateTeams);
    }

    public void UpdataCharacters(StoryCharacterModel characterModel)
    {
        currentDetailsModel.StoryCharacters.Add(characterModel);

        charactersView.Refresh(characterModel);
    }

    public void UpdateTeams(StoryTeamModel teamModel)
    {
        currentDetailsModel.TeamMembers.Add(teamModel);

        teamsView.Refresh(teamModel);
    }

    void ResetData()
    {
        currentDetailsModel = null;

        versionsView.ClearData();

        if (currentCreateScreen != null)
        {
            Destroy(currentCreateScreen);
        }

        currentObject?.SetActive(false);

        currentObject = null;

        buttonList[(int)currentType].color = disabledColor;

        currentType = EScreenSubType.Details;
    }
}
