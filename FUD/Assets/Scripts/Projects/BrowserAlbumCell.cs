using UnityEngine.UI;
using UnityEngine;
using System;

public class BrowserAlbumCell : MonoBehaviour
{
    public RawImage albumImage;

    public GameObject selectObject;

    public GameObject selectBG;


    PortfolioModel portfolioModel;

    Action<bool, PortfolioModel> OnSelectAction;

    Action<PortfolioModel> OnButtonAction;


    public void SetView(PortfolioModel portfolioModel, Action<bool, PortfolioModel> OnSelectAction, Action<PortfolioModel> OnButtonAction)
    {
        this.portfolioModel = portfolioModel;

        this.OnSelectAction = OnSelectAction;

        this.OnButtonAction = OnButtonAction;

        selectObject.SetActive(false);
    }

    public void OnAlbumButtonAction()
    {
        OnSelectAction?.Invoke(true, portfolioModel);
    }

    public void OnSelectToggleAction()
    {
        selectObject.SetActive(!selectObject.activeSelf);

        OnSelectAction?.Invoke(false, portfolioModel);
    }

    public void OnCellButtonAction()
    {
        OnButtonAction?.Invoke(portfolioModel);
    }
}
