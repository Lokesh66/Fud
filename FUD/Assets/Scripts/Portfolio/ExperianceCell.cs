using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ExperianceCell : MonoBehaviour
{
    public RectTransform content;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI locationText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI timeText;

    public GameObject mediaCell;


    WorkExperianceModel experianceModel;

    PortfolioView portfolioView;

    public void SetView(WorkExperianceModel experianceModel, PortfolioView portfolioView)
    {
        this.experianceModel = experianceModel;

        this.portfolioView = portfolioView;
    }

    public void OnButtonAction()
    {
        ShowAlbums();
    }

    void ShowAlbums()
    {
        List<WorkExpMedia> expMedia = experianceModel.WorkExpMedia;

        for (int i = 0; i < expMedia.Count; i++)
        {
            GameObject cellObject = Instantiate(mediaCell, content);

            cellObject.GetComponent<PortfolioMediaCell>().SetView(expMedia[i]);
        }
    }
}
