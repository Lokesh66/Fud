using UnityEngine.UI;
using UnityEngine;
using System;

public class PortfolioMediaCell : MonoBehaviour
{
    public Image albumImage;


    PortfolioModel albumModel;

    Action<PortfolioModel> OnButtonAction;

    public void SetView(PortfolioModel model,  Action<PortfolioModel> OnButtonAction)
    {
        this.albumModel = model;

        this.OnButtonAction = OnButtonAction;

        if (model.PortfolioMedia.Count > 0)
        {
            GameManager.Instance.downLoadManager.DownloadImage(model.PortfolioMedia[0].content_url, (sprite) =>
            {
                albumImage.sprite = sprite;
            });
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
