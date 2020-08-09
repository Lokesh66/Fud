using UnityEngine;
using TMPro;


public class StoryTeamDetails : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public UpdateTeamView updateTeamView;

    public StoryTeamDetailView detailView;


    StoryTeamModel teamModel;

    StoryTeamView teamsView;


    public void Load(StoryTeamModel teamModel, StoryTeamView teamView)
    {
        this.teamModel = teamModel;

        this.teamsView = teamView;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        //titleText.text = teamModel.title;
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        switch (buttonIndex)
        {
            case 0:
                OnEditButtonAction();
                break;
            case 1:
                OnDeleteButtonAction();
                break;
            case 2:
                OnCancelButtonAction();
                break;
            case 3:
                OnViewButtonAction();
                break;
        }
    }

    void OnCancelButtonAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void OnViewButtonAction()
    {
        detailView.Load(teamModel);

        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        updateTeamView.Load(teamModel, OnTeamUpdated);
    }

    void OnDeleteButtonAction()
    {
        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.RemoveTeam(teamModel.id, storyId, 8, (status) => {

            Debug.Log("status = " + status);

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Team Removed Successfully" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        teamsView.RemoveTeam(teamModel);
    }

    void Reset()
    {
        //titleText.text = string.Empty;
    }

    void OnTeamUpdated(StoryTeamModel updatedModel)
    {
        teamsView.UpdateTeam(teamModel, updatedModel);
    }
}
