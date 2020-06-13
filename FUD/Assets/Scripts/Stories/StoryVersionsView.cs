using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryVersionsView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public NoDataView noDataView;

    public StoryShareView shareView;

    public VersionDetailsView versionDetailsView;

    public VersionMultimediaView multimediaView;


    List<StoryVersion> storyVersionList = new List<StoryVersion>();

    StoryDetailsController detailsController;


    public void EnableView(List<StoryVersion> versionsList, StoryDetailsController detailsController)
    {
        gameObject.SetActive(true);

        this.detailsController = detailsController;

        if (storyVersionList?.Count > 0)
        {
           
        }
        else
        {
            storyVersionList = versionsList;

            SetView();
        }
    }

    void SetView()
    {
        noDataView.gameObject.SetActive(storyVersionList?.Count == 0);

        noDataView.SetView(GetNoDataModel());

        for (int i = 0; i < storyVersionList.Count; i++)
        {
            GameObject storyObject = Instantiate(storyCell, content);

            storyObject.GetComponent<StoryVersionCell>().SetView(storyVersionList[i], this);
        }
    }

    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        storyVersionList.Clear();
    }

    public void OnMediaButtonAction(StoryVersion storyVersion)
    {
        multimediaView.Load(storyVersion);
    }

    public void OnCellButtonAction(StoryVersion storyVersion)
    {
        versionDetailsView.Load(storyVersion, this);
    }

    public void OnShareButtonAction(StoryVersion storyVersion)
    {
        shareView.Load(storyVersion);
    }

    public void OnRemoveVersion(StoryVersion storyVersion)
    {
        int versionIndex = storyVersionList.IndexOf(storyVersion);

        Destroy(content.GetChild(versionIndex).gameObject);

        storyVersionList.Remove(storyVersion);
    }

    public void AddStoryVersion(StoryVersion storyVersion)
    {
        gameObject.SetActive(true);

        storyVersionList.Add(storyVersion);

        GameObject storyObject = Instantiate(storyCell, content);

        storyObject.GetComponent<StoryVersionCell>().SetView(storyVersion, this);
    }

    public void UpdateStoryVersion(StoryVersion storyVersion, StoryVersion updatedModel)
    {
        int versionIndex = storyVersionList.IndexOf(storyVersion);

        content.GetChild(versionIndex).GetComponent<StoryVersionCell>().SetView(updatedModel, this);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Story Versions Right Now";

        noDataModel.buttonName = "Add Story Version";

        noDataModel.buttonAction = detailsController.OnAddButtonAction;

        return noDataModel;
    }
}
