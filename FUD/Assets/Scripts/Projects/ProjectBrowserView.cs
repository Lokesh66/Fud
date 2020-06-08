using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectBrowserView : MonoBehaviour
{
    public RectTransform content;

    public ETabType tabType;

    public GameObject mediaCell;

    public NoDataView noDataView;


    List<PortfolioAlbumModel> albumModels;

    int pageNo = 1;

    public void Load()
    {
        gameObject.SetActive(true);

        content.DestroyChildrens();

        GetBrowserData();
    }

    void GetBrowserData()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetBrowserData(pageNo, (status, albums) => {

            if (status)
            {
                this.albumModels = albums;

                SetView();
            }
        });
    }

    void SetView()
    {
        noDataView.gameObject.SetActive(albumModels.Count == 0);

        if (noDataView.gameObject.activeSelf)
        {
            noDataView.SetView(GetNoDataModel());
        }

        for (int i = 0; i < albumModels.Count; i++)
        {
            EMediaType mediaType = DataManager.Instance.GetMediaType(albumModels[i].media_type);

            if (mediaType == EMediaType.Image)
            {
                GameObject mediaObject = Instantiate(mediaCell, content);

                mediaObject.GetComponent<CreatedPortfolioMediaCell>().SetView(albumModels[i].content_url);
            }
        }
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Albums Available";

        return noDataModel;
    }
}
