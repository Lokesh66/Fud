using UnityEngine;
using System;
using TMPro;


public class PortfolioMediaCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    PortfolioModel albumModel;

    Action<PortfolioModel> OnButtonAction;


    public void SetView(PortfolioModel model, Action<PortfolioModel> OnButtonAction)
    {
        this.albumModel = model;

        this.OnButtonAction = OnButtonAction;

        titleText.text = model.title;

        if (model.PortfolioMedia.Count > 0)
        {
            MultimediaModel albumModel = model.PortfolioMedia.Find(item => item.GetMediaType(item.media_type) == EMediaType.Image);

            if (albumModel != null)
            {
                model.onScreenModel = albumModel;
            }
        }
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(albumModel);
    }
}
