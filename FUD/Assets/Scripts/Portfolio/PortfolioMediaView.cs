using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioMediaView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    public PortfolioMediaDetails mediaDetailsView;


    List<PortfolioModel> portfolioModels;
    public void Load()
    { 
        GameManager.Instance.apiHandler.GetAllAlbums((status, models) =>
        {
            portfolioModels = models;

            SetView();
        });
    }

    void SetView()
    {
        content.DestroyChildrens();

        for (int i = 0; i < portfolioModels.Count; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            cellObject.GetComponent<PortfolioMediaCell>().SetView(portfolioModels[i], OnCellButtonAction);
        }
    }

    void OnCellButtonAction(PortfolioModel portfolioModel)
    {
        mediaDetailsView.Load(portfolioModel, this);
    }

    public void RemovePortfolio(PortfolioModel portfolioModel)
    {
        int modelIndex = portfolioModels.IndexOf(portfolioModel);

        Destroy(content.GetChild(modelIndex).gameObject);

        portfolioModels.Remove(portfolioModel);
    }
}
