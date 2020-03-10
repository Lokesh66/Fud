using UnityEngine;
using TMPro;


public class CreatedPortfolioView : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public RectTransform content;

    public GameObject mediaCell;


    public void SetView(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        content.DestroyChildrens();

        titleText.text = portfolioModel.title;

        descriptionText.text = portfolioModel.description;

        for (int i = 0; i < portfolioModel.PortfolioMedia.Count; i++)
        {
            if (portfolioModel.PortfolioMedia[i].media_type == "image")
            {
                GameObject mediaObject = Instantiate(mediaCell, content);

                mediaObject.GetComponent<CreatedPortfolioMediaCell>().SetView(portfolioModel.PortfolioMedia[i].content_url);
            }
        }
    }

    public void OnBackAction()
    {
        gameObject.SetActive(false);
    }
}
