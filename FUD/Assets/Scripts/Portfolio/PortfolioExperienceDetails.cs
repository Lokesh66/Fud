using UnityEngine;


public class PortfolioExperienceDetails : MonoBehaviour
{
    public UpdateExperienceView updatePortfolioExperiance;

    public WorkExperienceDetailView detailView;


    WorkExperianceModel experianceModel;

    PortfolioExperianceView experianceView;


    public void Load(WorkExperianceModel experianceModel, PortfolioExperianceView experianceView)
    {
        this.experianceModel = experianceModel;

        this.experianceView = experianceView;

        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        detailView.Load(experianceModel);
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
        }
    }

    void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        updatePortfolioExperiance.Load(experianceModel, OnExperienceUpdate);
    }

    void OnDeleteButtonAction()
    {
        GameManager.Instance.apiHandler.RemovePortfolioExperiance(experianceModel.id, 8, (status) => {

            if (status)
            {
                experianceView.RemovePortfolioExperience(experianceModel);
            }
        });
    }

    void OnExperienceUpdate(WorkExperianceModel updatedModel)
    {
        experianceView.OnUpdateExperience(experianceModel, updatedModel);
    }
}
