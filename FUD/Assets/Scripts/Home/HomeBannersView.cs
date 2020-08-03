using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HomeBannersView : MonoBehaviour
{
    public RectTransform bannerContent;

    public GameObject bannerCell;


    List<HomeBannerModel> bannerModels;


    public void Load()
    {
        GameManager.Instance.apiHandler.GetBanners(0, (status, response) =>
        {
            HomeBannersResponse responseModel = JsonUtility.FromJson<HomeBannersResponse>(response);

            if (status)
            {
                bannerModels = responseModel.data;

                SetView();
            }
        });
    }

    void SetView()
    {
        GameObject cellObject = null;

        for (int i = 0; i < bannerModels.Count; i++)
        {
            cellObject = Instantiate(bannerCell, bannerContent);

            cellObject.GetComponent<HomeBannerCell>().Load(bannerModels[i]);
        }
    }
}
