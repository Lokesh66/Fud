using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class UpdateSceneView : MonoBehaviour
{
    public UpdateSceneCharacterView dialoguesView;

    public TMP_Dropdown shootTimeDropdown;
    public TMP_Dropdown placeDropdown;  
    public TMP_InputField sceneOrderText;
    public TMP_InputField locationText;
    public TMP_InputField descriptionText;
    public TMP_Text startDateText;

    public TextMeshProUGUI buttonText;


    SceneDetailsModel detailsModel;

    string defaultDateText = "Select Date";

    List<Dictionary<string, object>> dialoguesList = new List<Dictionary<string, object>>();


    public void Load(SceneModel sceneModel, ProjectScenesPanel scenesPanel)
    {
        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetSceneDetails(sceneModel.id, (status, response) => {

            if (status)
            {
                SceneResponse responseModel = JsonUtility.FromJson<SceneResponse>(response);

                this.detailsModel = responseModel.data;

                SetView();
            }
        });
    }

    void SetView()
    {
        sceneOrderText.text = detailsModel.scene_order.ToString();

        locationText.text = detailsModel.location;

        descriptionText.text = detailsModel.description;

        shootTimeDropdown.value = shootTimeDropdown.options.FindIndex(option => option.text.Equals(detailsModel.shoot_time));

        placeDropdown.value = placeDropdown.options.FindIndex(option => option.text.Equals(detailsModel.place_type));
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

        dialoguesView.projectCharactersView.ClearData();
    }

    public void OnUpdateButtonAction()
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
            creationModel.project_id = detailsModel.project_id;
            creationModel.story_id = detailsModel.story_id;
            creationModel.story_version_id = detailsModel.story_version_id;
            creationModel.shoot_time = shootTime;
            creationModel.scene_order = int.Parse(sceneOrderText.text);
            creationModel.start_time = DateTime.Now.ToString();

            GameManager.Instance.apiHandler.UpdateProjectScene(creationModel, detailsModel.id, dialoguesView.GetSceneCharacters(), dialoguesView.GetDialogues(), (status, response) =>
            {
                OnAPIResponse(status, response);
            });
        }
    }

    void OnAPIResponse(bool status, string apiResponse)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(apiResponse);

        alertModel.message = status ? "Scene Updation Success" : responseModel.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        BackButtonAction();

        dialoguesView.ClearData();
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

    public void OnSaveDialogues(List<Dictionary<string, object>> dialogues)
    {
        dialoguesList.Clear();

        for (int i = 0; i < dialogues.Count; i++)
        {
            Dictionary<string, object> dialogueItem = new Dictionary<string, object>();

            foreach (var item in dialogues[i])
            {
                dialogueItem.Add(item.Key, item.Value);
            }

            dialoguesList.Add(dialogueItem);
        }
    }

    public void OnNextButtonAction()
    {
        dialoguesView.Load(detailsModel);
    }
}
