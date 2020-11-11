using UnityEngine;


public class PortfolioHandler : MonoBehaviour
{
    public PortfolioMediaView mediaView;

    public PortfolioExperianceView experianceView;


    PortfolioView portfolioView;


    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        gameObject.SetActive(true);

        ShowMedia();

        ShowExperianceScreen();
    }

    void ShowMedia()
    {
        mediaView.Load(portfolioView);
    }

    void ShowExperianceScreen()
    {
        experianceView.Load();
    }
}
