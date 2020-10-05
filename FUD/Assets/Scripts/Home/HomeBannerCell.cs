using UnityEngine;
using TMPro;


public class HomeBannerCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public UnityEngine.UI.Image bannerImage;

    public TextMeshProUGUI storyTitleText;

    public TextMeshProUGUI subTitleText;


    HomeBannerModel bannerModel;


    public void Load(HomeBannerModel bannerModel)
    {
        this.bannerModel = bannerModel;

        SetView();
    }

    void SetView()
    {
        Debug.Log("Screen width = " + CanvasManager.Instance.transform);

        rectTransform.sizeDelta = new Vector2(CanvasManager.Instance.GetCanvasWidth() * 3 / 4, rectTransform.sizeDelta.y);

        storyTitleText.text = bannerModel.title;

        subTitleText.text = bannerModel.sub_title;

        bannerImage.enabled = false;

        Loader.Instance.StartLoading();

        GameManager.Instance.apiHandler.DownloadImage(bannerModel.image_url, (sprite) => {

            Loader.Instance.StopLoading();

            if (this != null)
            {
                bannerImage.sprite = sprite;

                bannerImage.enabled = true;
            }
        });
    }
}
