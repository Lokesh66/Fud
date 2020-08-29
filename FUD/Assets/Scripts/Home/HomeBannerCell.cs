using UnityEngine;
using TMPro;


public class HomeBannerCell : MonoBehaviour
{
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
        storyTitleText.text = bannerModel.title;

        subTitleText.text = bannerModel.sub_title;

        Loader.Instance.StartLoading();

        GameManager.Instance.downLoadManager.DownloadImage(bannerModel.image_url, (sprite) => {

            Loader.Instance.StopLoading();

            bannerImage.sprite = sprite;
        });
    }
}
