using System.Collections.Generic;
using UnityEngine;

public class HomeStoriesPanel : MonoBehaviour
{
    public GameObject homeCell;
    public Transform parentContent;
    List<StoryModel> storiesList = new List<StoryModel>();

    private void OnEnable()
    {
        GetStories();
    }

    void GetStories()
    {
        storiesList = new List<StoryModel>();
        GameManager.Instance.apiHandler.GetAllStories((status, storiesList) => {

            if (status)
            {
                this.storiesList = storiesList;
            }

            SetView();
        });
    }

    void SetView()
    {
        foreach (Transform child in parentContent)
        {
            GameObject.Destroy(child.gameObject);
        }

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
                homeItem.SetView(HomeCellType.DEFAULT, i - 1, "", OnStorySelectAction);
            }
            else
            {
                homeItem.SetView(HomeCellType.STORY, i - 1, storiesList[i - 1].title, OnStorySelectAction);
            }
        }
    }

    void OnStorySelectAction(int index)
    {
        Debug.Log("OnStorySelectAction : " + index);

        if (index == -1)
        {
            StoryCreationView.Instance.Load(OnStoryScreenClosed);
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
