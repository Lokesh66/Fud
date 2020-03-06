using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;

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

    public Transform parentPanel;

    public TMP_Dropdown shootTimeDropdown;
    public TMP_Dropdown placeDropdown;
    public TMP_InputField sceneOrderText;
    public TMP_InputField locationText;
    public TMP_InputField descriptionText;
    public TMP_Text startDateText;

    public TMP_Text errorText;

    bool isNewSceneCreated;

    Project projectModel;

    string defaultDateText = "Select Date";

    Action<bool> backAction;

    public void SetView(Project projectModel, Action<bool> action)
    {
        this.projectModel = projectModel;
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewSceneCreated = false;
    }

    public void OnAuditionTypeSelectedAction()
    {
       /* Debug.Log(shootTimeDropdown.captionText.text);
        if (shootTimeDropdown.captionText.text.ToLower().Equals("group"))
        {
            membersText.gameObject.SetActive(true);
        }
        else
        {
            membersText.gameObject.SetActive(false);
        }*/
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
        backAction = null;
    }

    public void CreateSceneButtonAction()
    {
        string errorMessage = string.Empty;

        //Call an API to add into audition list
        if (string.IsNullOrEmpty(sceneOrderText.text))
        {
            errorMessage = "Scene order should not be empty";
            //ShowErrorMessage("Audition type should not be empty");
        }       
        else if (string.IsNullOrEmpty(locationText.text))
        {
            errorMessage = "Scene location should not be empty";
            //ShowErrorMessage("Audition title should not be empty");
        }
        else if (string.IsNullOrEmpty(startDateText.text) || startDateText.text.Equals(defaultDateText))
        {
            errorMessage = "Start date should not be empty";          
            //ShowErrorMessage("Audition date should not be empty");
        }
        else if (string.IsNullOrEmpty(descriptionText.text))
        {
            errorMessage = "Scene description should not be empty";
            //ShowErrorMessage("Audition description should not be empty");
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            CanvasManager.Instance.alertView.ShowAlert(alertModel); 
            return;
        }

        string selectedPlace = placeDropdown.options[placeDropdown.value].text;

        string shootTime = shootTimeDropdown.options[shootTimeDropdown.value].text;

        SceneCreationModel creationModel = new SceneCreationModel();

        creationModel.scene_characters = new List<SceneCharacter>();

        List<Dictionary<string, object>> storyScenes = new List<Dictionary<string, object>>();

        Dictionary<string, object> kvp = new Dictionary<string, object>();

        kvp.Add("character_id", 1);

        kvp.Add("dailogue", "Hello hero");

        creationModel.decsription = descriptionText.text;
        creationModel.location = locationText.text;
        creationModel.place_type = selectedPlace;
        creationModel.project_id = projectModel.id;
        creationModel.story_id = projectModel.story_id;
        creationModel.story_version_id = projectModel.story_version_id;
        creationModel.shoot_time = shootTime;
        creationModel.scene_order = int.Parse(sceneOrderText.text);
        creationModel.start_time = startDateText.text;

        storyScenes.Add(kvp);

        GameManager.Instance.apiHandler.CreateProjectScene(creationModel, storyScenes, (status, response) => {
            Debug.Log("OnCreateAudition : "+response);
            if (status)
            {
                isNewSceneCreated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Scene Created Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                alertModel.canEnableTick = true;
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Creating Scene Failed";
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
        });
    }

    void ShowErrorMessage(string message)
    {
        errorText.text = message;
        if (IsInvoking("DisableErrorMessage"))
            CancelInvoke("DisableErrorMessage");
        Invoke("DisableErrorMessage", 2.0f);
    }

    void DisableErrorMessage()
    {
        errorText.DOFade(0f,0.5f).OnComplete(() => {
            errorText.text = string.Empty;
            errorText.color = Color.red;
        });
    }
}
