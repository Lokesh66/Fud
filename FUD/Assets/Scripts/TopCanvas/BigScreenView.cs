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
        Texture2D texture = GameManager.Instance.downLoadManager.GetLocalTexture(imageURL); 

        TextureScale.ThreadedScale(texture, Screen.width, Screen.height, true);

        Sprite targetSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        selectedImage.sprite = targetSprite;
    }

    public void OnCloseButtonAction()
    {
        gameObject.SetActive(false);
    }
}
