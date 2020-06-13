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
            EMediaType mediaType = DataManager.Instance.GetMediaType(portfolioModel.PortfolioMedia[i].media_type);

            if (mediaType == EMediaType.Image)
            {
                GameObject mediaObject = Instantiate(mediaCell, content);

                mediaObject.GetComponent<CreatedPortfolioMediaCell>().SetView(portfolioModel.PortfolioMedia[i], OnDeleteAction);
            }
        }
    }

    public void OnBackAction()
    {
        gameObject.SetActive(false);
    }

    void OnDeleteAction(PortfolioAlbumModel albumModel)
    {

    }
}
