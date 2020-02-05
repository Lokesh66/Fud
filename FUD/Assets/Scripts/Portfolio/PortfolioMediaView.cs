using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioMediaView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    List<PortfolioAlbumModel> portfolioAlbums;
    public void Load()
    {
        if (portfolioAlbums != null)
        {
            SetView();
        }
        else
        {
            GameManager.Instance.apiHandler.GetAllAlbums((status, models) =>
            {
                portfolioAlbums = models;

                SetView();
            });
        }
    }

    void SetView()
    {
        for (int i = 0; i < portfolioAlbums.Count; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            cellObject.GetComponent<PortfolioMediaCell>().SetView(portfolioAlbums[i]);
        }
    }
}
