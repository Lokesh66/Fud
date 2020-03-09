using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class PortfolioExperienceDetails : MonoBehaviour
{
    //public Image userImage;

    //public TextMeshProUGUI titleText;

    //public TextMeshProUGUI description;

    public UpdateExperienceView updatePortfolioExperiance;


    WorkExperianceModel experianceModel;

    PortfolioExperianceView experianceView;


    public void Load(WorkExperianceModel experianceModel, PortfolioExperianceView experianceView)
    {
        this.experianceModel = experianceModel;

        this.experianceView = experianceView;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        //titleText.text = experianceModel.title;

        //description.text = experianceModel.description;
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
        Reset();

        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        updatePortfolioExperiance.Load(experianceModel);
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

    void Reset()
    {
        /*description.text = string.Empty;

        userImage.sprite = null;*/
    }
}
