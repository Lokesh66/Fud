using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProjectSceneView : MonoBehaviour
{
    public SceneCharacterView charactersView;

    public TMP_Dropdown shootTimeDropdown;
    public TMP_Dropdown placeDropdown;
    public TMP_InputField sceneOrderText;
    public TMP_InputField locationText;
    public TMP_InputField descriptionText;
    public TMP_Text startDateText;


    ProjectScenesPanel scenesPanel;

    SceneModel sceneModel;

    SceneDetailsModel detailsModel;

    List<SceneCharacterBody> dialoguesList = new List<SceneCharacterBody>();


    public void Load(SceneModel sceneModel, ProjectScenesPanel scenesPanel)
    {
        this.sceneModel = sceneModel;
        
        this.scenesPanel = scenesPanel;

        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetSceneDetails(sceneModel.id, (status, response) => {

            if (status)
            {
                SceneResponse responseModel = JsonUtility.FromJson<SceneResponse>(response);

                detailsModel = responseModel.data;

                SetView();
            }
        });
    }

    void SetView()
    {
        sceneOrderText.text = detailsModel.scene_order.ToString();

        locationText.text = detailsModel.location;

        descriptionText.text = detailsModel.description;

        startDateText.text = detailsModel.start_time.ToString();

        shootTimeDropdown.captionText.text = detailsModel.shoot_time;

        placeDropdown.captionText.text = detailsModel.place_type;
    }

    public void BackButtonAction()
    {
        ResetData();

        gameObject.SetActive(false);
    }

    public void OnNextButtonAction()
    {
        charactersView.Load(detailsModel.SceneCharacters);
    }

    void ResetData()
    {
        sceneOrderText.text = locationText.text = descriptionText.text = startDateText.text = string.Empty;

        shootTimeDropdown.captionText.text = placeDropdown.captionText.text = string.Empty;
    }
}
