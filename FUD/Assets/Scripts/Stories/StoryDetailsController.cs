﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryDetailsController : MonoBehaviour
{
    public enum EScreenSubType
    {
        Details,
        Versions,
        Characters,
        Team
    }

    public Transform creationPanelParent;

    public StoryDetailsView storyDetailsView;

    public StoryVersionsView versionsView;

    public StoryCharactersView charactersView;

    public StoryTeamView teamsView;

    public TextMeshProUGUI titeText;

    public TextMeshProUGUI[] buttonList;

    public GameObject storyCreationCache;

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


    MyStoriesController storiesController;


    public void Load(StoryDetailsModel detailsModel, MyStoriesController storiesController)
    {
        currentDetailsModel = detailsModel;

        this.storiesController = storiesController;

        UpdateScreen();
    }

    public void EnableView(StoryDetailsModel detailsModel)
    {
        currentDetailsModel = detailsModel;
    }

    void SetData()
    { 
        
    }

    public void OnBackButtonAction()
    {
        Destroy(currentCreateScreen);

        storiesController?.OnRemoveLastSubView();
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

        charactersView.Load(currentDetailsModel.StoryCharacters, this);
    }

    void ShowTeamsScreen()
    {
        currentObject = teamsView.gameObject;

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
        GameObject createObject = Instantiate(updateStoryCache, creationPanelParent);

        currentCreateScreen = createObject;

        createObject.GetComponent<StoryUpdateView>().Load(this);
    }

    void CreateStoryVersion()
    {
        //GameObject storyVersionObject = Instantiate(storyCreationCache, creationPanelParent);

        //storiesController.CreateSubView(storyVersionObject);

        //currentCreateScreen = storyVersionObject;

        //story
    }

    void CreateCharacter()
    {
        GameObject addCharacterObject = Instantiate(addCharacterCache, creationPanelParent);

        //storiesController.CreateSubView(addCharacterObject);
        currentCreateScreen = addCharacterObject;

        addCharacterObject.GetComponent<CreateCharacterView>().SetView(currentDetailsModel, UpdataCharacters);
    }

    void AddTeamMember()
    {
        GameObject addTeamObject = Instantiate(addTeamMemberCache, creationPanelParent);

        //storiesController.CreateSubView(addTeamObject);
        currentCreateScreen = addTeamObject;

        addTeamObject.GetComponent<UpdateTeamView>().SetView(currentDetailsModel);
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
}
