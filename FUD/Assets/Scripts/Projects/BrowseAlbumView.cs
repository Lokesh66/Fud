using System.Collections.Generic;
using UnityEngine;


public class BrowseAlbumView : MonoBehaviour
{
    public RectTransform cellContent;

    public GameObject mediaCell;


    PortfolioModel portfolioModel;

    List<MultimediaModel> albumModels;

    List<BrowserMediaCell> mediaCells = new List<BrowserMediaCell>();


    public void Load(PortfolioModel portfolioModel)
    {
        this.portfolioModel = portfolioModel;

        gameObject.SetActive(true);

        SetView();
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);

        mediaCells.Clear();
    }

    void SetView()
    {
        albumModels = portfolioModel.PortfolioMedia;

        GameObject cellObject;

        BrowserMediaCell _mediaCell;

        for (int i = 0; i < albumModels.Count; i++)
        {
            cellObject = Instantiate(mediaCell, cellContent);

            _mediaCell = cellObject.GetComponent<BrowserMediaCell>();

            _mediaCell.SetView(albumModels[i]);

            mediaCells.Add(_mediaCell);
        }
    }

    void SetVideoThumbnails(int index)
    {
        mediaCells[index].SetVideoThumbnail(() => {

            index++;

            if (index >= mediaCells.Count)
            {
                return;
            }

            SetVideoThumbnails(index);
        });
    }
}
