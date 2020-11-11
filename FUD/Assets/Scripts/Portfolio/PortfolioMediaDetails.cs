using UnityEngine;


public class PortfolioMediaDetails : MonoBehaviour
{
    public UpdatePortfolioView updateView;

    public PortfolioAlbumView albumView;


    PortfolioModel portfolioModel;

    PortfolioMediaView mediaView;


    public void Load(PortfolioModel portfolioModel, PortfolioMediaView mediaView)
    {
        this.portfolioModel = portfolioModel;

        this.mediaView = mediaView;

        gameObject.SetActive(true);
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
                OnViewButtonAction();
                break;
            case 2:
                OnShareButtonAction();
                break;
            case 3:
                OnDeleteButtonAction();
                break;
            case 4:
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
        updateView.Load(portfolioModel);
    }

    void OnViewButtonAction()
    {
        albumView.Load( portfolioModel);

        gameObject.SetActive(false);
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
}
