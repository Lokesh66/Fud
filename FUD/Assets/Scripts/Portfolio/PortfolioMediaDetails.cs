using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PortfolioMediaDetails : MonoBehaviour
{
    public Image userImage;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    public UpdatePortfolioView updateView;


    PortfolioModel portfolioModel;

    PortfolioMediaView mediaView;


    public void Load(PortfolioModel portfolioModel, PortfolioMediaView mediaView)
    {
        this.portfolioModel = portfolioModel;

        this.mediaView = mediaView;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        //titleText.text = portfolioModel.title;

        //description.text = portfolioModel.description;
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
                OnShareButtonAction();
                break;
            case 2:
                OnDeleteButtonAction();
                break;
            case 3:
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
        updateView.Load(portfolioModel);
    }

    void OnDeleteButtonAction()
    {
        GameManager.Instance.apiHandler.RemovePortfolio(portfolioModel.id, 8, (status) => {

            if (status)
            {
                mediaView.RemovePortfolio(portfolioModel);
            }
        });
    }

    void OnShareButtonAction()
    {
        mediaView.OnShareButtonAction(portfolioModel);
    }

    void Reset()
    {
        //description.text = string.Empty;

        //userImage.sprite = null;
    }
}
