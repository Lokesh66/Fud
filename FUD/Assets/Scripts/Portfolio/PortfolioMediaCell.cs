using UnityEngine.UI;
using UnityEngine;
using System;

public class PortfolioMediaCell : MonoBehaviour
{

    PortfolioModel albumModel;

    Action<PortfolioModel> OnButtonAction;

    public void SetView(PortfolioModel model,  Action<PortfolioModel> OnButtonAction)
    {
        this.albumModel = model;

        this.OnButtonAction = OnButtonAction;

        if (model.PortfolioMedia.Count > 0)
        {
            PortfolioAlbumModel albumModel = model.PortfolioMedia.Find(item => item.GetMediaType(item.media_type) == EMediaType.Image);

            if (albumModel != null)
            {
                model.onScreenModel = albumModel;
            }
        }
    }

    public void OnShareAction()
    { 
    
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(albumModel);
    }
}
