using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CreateScenesView : MonoBehaviour
{
    #region Singleton

    private static CreateScenesView instance = null;

    private CreateScenesView()
    {

    }

    public static CreateScenesView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CreateScenesView>();
            }
            return instance;
        }
    }
    #endregion

    public CreateDialoguesView dialoguesView;

    public Transform parentPanel;

    public TMP_Dropdown shootTimeDropdown;
    public TMP_Dropdown placeDropdown;
    public TMP_InputField sceneOrderText;
    public TMP_InputField locationText;
    public TMP_InputField descriptionText;
    public TMP_Text startDateText;

    bool isNewSceneCreated;

    bool isDialoguesAdded = false;

    Project projectModel;

    string defaultDateText = "Select Date";

    Action<bool> backAction;

    List<Dictionary<string, object>> autoSceneDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, object>> dialoguesList = new List<Dictionary<string, object>>();


    public void SetView(Project projectModel, Action<bool> action)
    {
        this.projectModel = projectModel;
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewSceneCreated = false;

        isDialoguesAdded = false;
    }

    public void OnDateSelectAction()
    {
        DatePicker.Instance.GetDate(DateTime.Now, DateTime.Now, DateTime.MaxValue, (date, dateString) =>
        {
            startDateText.text = string.IsNullOrEmpty(dateString) ? defaultDateText : dateString;
        });
    }

    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isNewSceneCreated);
        ClearData();
        backAction = null;
    }

    public void OnNextButtonAction()
    {
        ShowDialougesPanel();
    }

    public void CreateSceneButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }
        else
        {
            string selectedPlace = placeDropdown.options[placeDropdown.value].text;

            string shootTime = shootTimeDropdown.options[shootTimeDropdown.value].text;

            SceneCreationModel creationModel = new SceneCreationModel();

            creationModel.decsription = descriptionText.text;
            creationModel.location = locationText.text;
            creationModel.place_type = selectedPlace;
            creationModel.project_id = projectModel.id;
            creationModel.story_id = projectModel.story_id;
            creationModel.story_version_id = projectModel.story_version_id;
            creationModel.shoot_time = shootTime;
            creationModel.scene_order = int.Parse(sceneOrderText.text);
            creationModel.start_time = startDateText.text;

            GameManager.Instance.apiHandler.CreateProjectScene(creationModel, dialoguesView.GetSceneCharacters(), dialoguesView.GetDialogues(), (status, response) =>
            {
                OnAPIResponse(status, response);
            });
        }
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Scene Creation Success" : baseResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        isNewSceneCreated = true;

        BackButtonAction();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        //Call an API to add into audition list
        if (string.IsNullOrEmpty(sceneOrderText.text))
        {
            errorMessage = "Scene order should not be empty";
        }
        else if (string.IsNullOrEmpty(locationText.text))
        {
            errorMessage = "Scene location should not be empty";
        }
        else if (string.IsNullOrEmpty(startDateText.text) || startDateText.text.Equals(defaultDateText))
        {
            errorMessage = "Start date should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionText.text))
        {
            errorMessage = "Scene description should not be empty";
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

    void ShowDialougesPanel()
    {
        dialoguesView.Load(projectModel.id);
    }

    void ClearData()
    {
        placeDropdown.value = shootTimeDropdown.value = 0;

        sceneOrderText.text = locationText.text = descriptionText.text = string.Empty;

        startDateText.text = string.Empty;

        dialoguesView.projectCharactersView.ClearData();
    }
}
