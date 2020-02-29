using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionMediaCell : MonoBehaviour
{
    public Image albumImage;


    MultimediaModel albumModel;
    public void SetView(MultimediaModel model)
    {
        this.albumModel = model;

        GameManager.Instance.downLoadManager.DownloadImage(model.content_url, (sprite) => {

            albumImage.sprite = sprite;
        });
    }

    public void OnShareAction()
    {

    }
}
