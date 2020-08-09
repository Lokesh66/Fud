using System.Text.RegularExpressions;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using TMPro;

public class ProjectCreationView : MonoBehaviour
{
    #region Singleton

    private static ProjectCreationView instance = null;

    private ProjectCreationView()
    {

    }

    public static ProjectCreationView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ProjectCreationView>();
            }
            return instance;
        }
    }
    #endregion

    public Transform parentPanel;

    public TMP_InputField titleField;

    public TMP_InputField budgetField;

    public TMP_InputField descriptionField;
    
    public TMP_Dropdown storySelectionDropdown;

    public TMP_Text startDateText;

    public TMP_Text releaseDateText;

    public TMP_Text errorText;



    Action<bool> backAction;

    string defaultDateText = "Select Date";

    bool isDataUpdated = false;

    List<ProjectStory> stories = new List<ProjectStory>();

    public void SetView(List<ProjectStory> stories, Action<bool> action)
    {
        this.stories = stories;

        storySelectionDropdown.options.Clear();

        foreach (ProjectStory story in stories)
        {
            storySelectionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = story.title });
        }

        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isDataUpdated = false;
    }

    public void OnStartDateAction()
    {
        DatePicker.Instance.GetDate(DateTime.Now, DateTime.Now, DateTime.MaxValue, (date, dateString) =>
        {
            if (!string.IsNullOrEmpty(dateString))
                startDateText.text = dateString;
        });
    }

    public void OnReleaseDateAction()
    {
        DatePicker.Instance.GetDate(DateTime.Now, DateTime.Now, DateTime.MaxValue, (date, dateString) =>
        {
            if (!string.IsNullOrEmpty(dateString))
                releaseDateText.text = dateString;
        });
    }

    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isDataUpdated);
        ClearData();
        backAction = null;
    }

    public void OnSubmitProject()
    {
        if (!CanCallAPI())
        {
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", titleField.text);

        ProjectStory model = stories.Find(item => item.title.Equals(storySelectionDropdown.captionText.text));

        parameters.Add("story_id", model.story_id);

        parameters.Add("story_version_id", model.StoryVersions.id);

        parameters.Add("cost_estimation", long.Parse(budgetField.text));

        parameters.Add("start_date", startDateText.text);

        parameters.Add("release_date", releaseDateText.text);

        parameters.Add("description", descriptionField.text);

        GameManager.Instance.apiHandler.CreateProject(parameters, (status, response) => {

            if (status)
            {
                Debug.Log("Project Created Successfully");
                isDataUpdated = true;

                AlertModel alertModel = new AlertModel();
                alertModel.message = "Project Created Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                Debug.LogError("Project Failed To Save");
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Project Creation Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(titleField.text))
        {
            errorMessage = "Name your project, should not be empty";
        }
        else if (string.IsNullOrEmpty(storySelectionDropdown.captionText.text))
        {
            errorMessage = "Select Stroy for your story";
        }
        else if (string.IsNullOrEmpty(budgetField.text))
        {
            errorMessage = "Select budjet for your story";
        }
        else if (string.IsNullOrEmpty(releaseDateText.text) || startDateText.text.Equals(defaultDateText))
        {
            errorMessage = "Please select respective dates";
            //ShowErrorMessage("Audition date should not be empty");
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Add description for your Project";
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

    void ClearData()
    {
        titleField.text = budgetField.text = descriptionField.text = string.Empty;

        storySelectionDropdown.value = 0;
    }
}
