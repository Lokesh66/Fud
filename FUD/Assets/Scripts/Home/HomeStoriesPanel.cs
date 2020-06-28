using System.Collections.Generic;
using UnityEngine;


public class HomeStoriesPanel : MonoBehaviour
{
    public GameObject homeCell;
    public Transform parentContent;
    List<StoryModel> storiesList = new List<StoryModel>();

    int pageNo = 0;

    private void OnEnable()
    {
        GetStories();
    }

    void GetStories()
    {
        storiesList = new List<StoryModel>();

        GameManager.Instance.apiHandler.GetAllStories(pageNo, (status, storiesList) => {

            if (status)
            {
                this.storiesList = storiesList;
            }

            SetView();
        });
    }

    void SetView()
    {
        parentContent.DestroyChildrens();

        if (storiesList == null)
        {
            storiesList = new List<StoryModel>();
        }

        for(int i = 0; i <= storiesList.Count; i++)
        {
            GameObject storyObject = Instantiate(homeCell, parentContent);

            HomeCell homeItem = storyObject.GetComponent<HomeCell>();

            if (i == 0)
            {
                homeItem.SetView(HomeCellType.DEFAULT, i - 1, "", storiesList[i].story_poster, OnStorySelectAction);
            }
            else
            {
                homeItem.SetView(HomeCellType.STORY, i - 1, storiesList[i - 1].title, storiesList[i - 1].story_poster, OnStorySelectAction);
            }
        }

        if (storiesList.Count == 0)
        {
            GameObject storyObject = Instantiate(homeCell, parentContent);

            HomeCell homeItem = storyObject.GetComponent<HomeCell>();

            homeItem.SetView(HomeCellType.DEFAULT, -1, string.Empty, string.Empty, OnStorySelectAction);
        }
    }

    void OnStorySelectAction(int index)
    {
        if (index == -1)
        {
            if (DataManager.Instance.CanLoadScreen(EFeatureType.StoryCreation))
            {
                StoryCreationView.Instance.Load(OnStoryScreenClosed);
            }
            else
            {
                UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.StoryCreation);
            }
        }
        else
        {
            //Open story of storieslist[index]
            StoryDetailsController.Instance.Load(storiesList[0].id, OnStoryScreenClosed);
        }
    }

    void OnStoryScreenClosed()
    {

    }
}
