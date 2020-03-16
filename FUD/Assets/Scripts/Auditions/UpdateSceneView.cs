using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class UpdateSceneView : MonoBehaviour
{
    public CharacterDialoguesView dialoguesView;

    public Transform parentPanel;

    public TMP_Dropdown shootTimeDropdown;
    public TMP_Dropdown placeDropdown;
    public TMP_InputField sceneOrderText;
    public TMP_InputField locationText;
    public TMP_InputField descriptionText;
    public TMP_Text startDateText;

    public TextMeshProUGUI buttonText;

    public TMP_Text errorText;


    ProjectScenesPanel scenesPanel;

    bool isNewSceneCreated;

    bool isDialoguesAdded = false;

    SceneModel sceneModel;

    string defaultDateText = "Select Date";

    List<Dictionary<string, object>> dialoguesList = new List<Dictionary<string, object>>();


    public void SetView(SceneModel sceneModel, ProjectScenesPanel scenesPanel)
    {
        this.sceneModel = sceneModel;
        parentPanel.gameObject.SetActive(true);
        this.scenesPanel = scenesPanel;
        isNewSceneCreated = false;

        isDialoguesAdded = false;

        buttonText.text = "Next";

        SetView();
    }

    void SetView()
    { 
    
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
        gameObject.SetActive(false);
    }

    public void CreateSceneButtonAction()
    {
        if (!isDialoguesAdded)
        {
            ShowDialougesPanel();
        }
        else
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

                creationModel.scene_characters = new List<SceneCharacter>();

                creationModel.decsription = descriptionText.text;
                creationModel.location = locationText.text;
                creationModel.place_type = selectedPlace;
                /*creationModel.project_id = projectModel.id;
                creationModel.story_id = projectModel.story_id;
                creationModel.story_version_id = projectModel.story_version_id;*/
                creationModel.shoot_time = shootTime;
                creationModel.scene_order = int.Parse(sceneOrderText.text);
                creationModel.start_time = startDateText.text;

                GameManager.Instance.apiHandler.CreateProjectScene(creationModel, dialoguesList, (status, response) =>
                {
                    OnAPIResponse(status);
                });
            }
        }
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
            return false;
        }

        return true;
    }

    void ShowDialougesPanel()
    {
        dialoguesView.EnableView();
    }

    public void OnSaveDialogues(List<Dictionary<string, object>> dialogues)
    {
        dialoguesList = dialogues;

        if (dialoguesList.Count > 0)
        {
            buttonText.text = "Create";

            isDialoguesAdded = true;
        }
    }
}
