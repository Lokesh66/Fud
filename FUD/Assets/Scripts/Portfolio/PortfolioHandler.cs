using UnityEngine;


public class PortfolioHandler : MonoBehaviour
{
    public PortfolioBasicInfo basicInfoView;

    public PortfolioMediaView mediaView;

    public PortfolioExperianceView experianceView;

    public ProfileInfoView infoView;


    PortfolioView portfolioView;


    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        gameObject.SetActive(true);

        ShowMedia();

        ShowExperianceScreen();

        //ShowBasicInfo();
    }

    void ShowBasicInfo()
    {
        basicInfoView.Load();
    }

    void ShowMedia()
    {
        mediaView.Load(portfolioView);
    }

    void ShowExperianceScreen()
    {
        experianceView.Load(this);
    }

    public void OnRemoveSubScreen()
    {
        gameObject.SetActive(true);
    }

    public void OnEditButtonAction()
    {
        infoView.Load(OnPortifolioClose);
    }

    void OnPortifolioClose(bool isDataUpdated)
    {
        if (isDataUpdated)
        {
            ShowBasicInfo();
        }
    }    
}
