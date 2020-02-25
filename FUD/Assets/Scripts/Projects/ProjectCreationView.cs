using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

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

    public TMP_Dropdown durationDropDown;

    public TMP_Text errorText;

    System.Action<bool> backAction;

    bool isDataUpdated = false;

    List<ProjectStory> stories = new List<ProjectStory>();

    public void SetView(List<ProjectStory> stories, System.Action<bool> action)
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

    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isDataUpdated);
        backAction = null;
    }

    public void OnSubmitProject()
    {
        if (string.IsNullOrEmpty(titleField.text))
        {
            ShowErrorMessage("Name your project, should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(storySelectionDropdown.captionText.text))
        {
            ShowErrorMessage("Select Stroy for your story");
            return;
        }
        if (string.IsNullOrEmpty(budgetField.text))
        {
            ShowErrorMessage("Select budjet for your story");
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", titleField.text);

        ProjectStory model = stories.Find(item => item.title.Equals(storySelectionDropdown.captionText.text));

        parameters.Add("story_id", model.story_id);

        parameters.Add("story_version_id", model.StoryVersions.id);

        parameters.Add("cost_estimation", long.Parse(budgetField.text));

        parameters.Add("estimated_time", 1609818627);// long.Parse(durationDropDown.captionText.text));

        GameManager.Instance.apiHandler.CreateProject(parameters, (status, response) => {

            if (status)
            {
                Debug.Log("Project Created Successfully");
                isDataUpdated = true;
                BackButtonAction();
            }
            else
            {
                Debug.LogError("Project Failed To Save");
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
        errorText.DOFade(0f, 0.5f).OnComplete(() => {
            errorText.text = string.Empty;
            errorText.color = Color.red;
        });
    }
}
