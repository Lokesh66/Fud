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

    EMediaType mediaType;


    public void SetView(PortfolioModel portfolioModel, Action<bool, PortfolioModel> OnSelectAction, Action<PortfolioModel> OnButtonAction)
    {
        this.portfolioModel = portfolioModel;

        this.OnSelectAction = OnSelectAction;

        this.OnButtonAction = OnButtonAction;

        //selectBG.SetActive(portfolioModel.onScreenModel.content_url.IsNOTNullOrEmpty());
        
        //MultimediaModel _albumModel = portfolioModel.PortfolioMedia.Find(item => DataManager.Instance.GetMediaType(item.media_type) == EMediaType.Video);

        //if (_albumModel != null)
        //{
        //    portfolioModel.onScreenModel = albumModel;

        //    SetVideoThumbnail(null);
        //}
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
