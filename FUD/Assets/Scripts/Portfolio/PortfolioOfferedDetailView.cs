using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PortfolioOfferedDetailView : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descripionText;

    public GameObject mediaCell;

    public RectTransform mediaContent;


    PortfolioActivityModel activityModel;

    List<PortfolioAlbumModel> mediaList;

    List<CreatedPortfolioMediaCell> mediaCells = new List<CreatedPortfolioMediaCell>();


    public void Load(PortfolioActivityModel activityModel)
    {
        this.activityModel = activityModel;

        gameObject.SetActive(true);

        titleText.text = activityModel.Portfolio.title;

        descripionText.text = activityModel.Portfolio.description;

        mediaList = activityModel.Portfolio.PortfolioMedia;

        LoadMedia();
    }

    void LoadMedia()
    {
        mediaContent.DestroyChildrens();

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            CreatedPortfolioMediaCell _mediaCell = mediaObject.GetComponent<CreatedPortfolioMediaCell>();

            _mediaCell.SetView(mediaList[i], null);

            mediaCells.Add(_mediaCell);
        }

        if (mediaList.Count > 0)
        {
            SetVideoThumbnails(0);
        }
    }

    void SetVideoThumbnails(int index)
    {
        mediaCells[index].SetVideoThumbnail(() => {

            index++;

            if (index >= mediaList.Count)
            {
                return;
            }

            SetVideoThumbnails(index);
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
