using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class StoryCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    public GameObject updateStoryCache;

    public GameObject readMoreObject;

    public GameObject editObject;

    public GameObject statusTag;

    public Image storyImage;


    StoryModel storyModel;

    ScrollRect scrollRect;

    public Action<int> OnTapActon;

    Vector2 startPoint;


    public void SetView(StoryModel storyModel, Action<int> tapAction = null)
    {
        this.storyModel = storyModel;

        this.OnTapActon = tapAction;

        titleText.text = storyModel.title;

        description.text = storyModel.description;

        editObject.SetActive(true);

        //SetImage();
    }

    public void OnButtonAction()
    {
        OnTapActon?.Invoke(storyModel.id);
    }

    public void OnReadMoreButtonAction()
    {
        description.overflowMode = TextOverflowModes.Overflow;

        readMoreObject.SetActive(false);

        Vector2 cellSize = rectTransform.sizeDelta;

        cellSize.y += description.preferredHeight - 120.0f;

        rectTransform.sizeDelta = new Vector2(cellSize.x, cellSize.y);
    }

    public void OnUpdateButtonAction()
    {
        Transform parent = StoryDetailsController.Instance.transform;

        GameObject createObject = Instantiate(updateStoryCache, parent);

        createObject.GetComponent<StoryUpdateView>().Load(storyModel, this);
    }

    void SetImage()
    {
        string imagePath = string.Empty;

        Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

        TextureScale.ThreadedScale(texture, (int)storyImage.rectTransform.rect.width, (int)storyImage.rectTransform.rect.height, true);

        storyImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
