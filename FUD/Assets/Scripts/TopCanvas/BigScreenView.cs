using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class BigScreenView : MonoBehaviour
{
    public Image selectedImage;


    public void Load(string imageURL)
    {
        gameObject.SetActive(true);

        SetView(imageURL);
    }

    void SetView(string imageURL)
    {
        GameManager.Instance.apiHandler.DownloadImage(imageURL, (sprite) => { 

            if (sprite != null)
            {
                TextureScale.ThreadedScale(sprite.texture, Screen.width, Screen.height, true);

                Sprite targetSprite = Sprite.Create(sprite.texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), new Vector2(0.5f, 0.5f));

                selectedImage.sprite = targetSprite;
            }
        });
    }

    public void OnCloseButtonAction()
    {
        gameObject.SetActive(false);
    }
}
