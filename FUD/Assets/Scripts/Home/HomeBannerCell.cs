using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using UnityEngine;
using TMPro;


public class HomeBannerCell : MonoBehaviour
{
    public RemoteImageBehaviour bannerImage;

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

        bannerImage.Load(bannerModel.image_url);
    }
}
