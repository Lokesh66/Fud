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
    }

    public void OnAlbumButtonAction()
    {
        OnSelectAction?.Invoke(true, portfolioModel);
    }

    public void OnSelectToggleAction(Toggle toggle)
    {
        selectObject.SetActive(toggle.isOn);

        OnSelectAction?.Invoke(false, portfolioModel);
    }

    public void OnCellButtonAction()
    {
        OnButtonAction?.Invoke(portfolioModel);
    }
}
