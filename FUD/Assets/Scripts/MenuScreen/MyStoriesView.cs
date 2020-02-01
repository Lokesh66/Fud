
using System.Collections.Generic;
using UnityEngine;


public class MyStoriesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public GameObject detailsPanel;


    List<StoryModel> storiesList;

    MyStoriesController storiesController;

    public void Load()
    {
        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetAllStories((status, storiesList) => {

            if (status)
            {
                this.storiesList = storiesList;

                SetView();
            }
        });
    }

    public void EnableView(MyStoriesController storiesController)
    {
        this.storiesController = storiesController;

        if (storiesList?.Count > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            Load();
        }
    }

    void SetView()
    {
        for (int i = 0; i < storiesList.Count; i++)
        {
            GameObject storyObject = Instantiate(storyCell, content);

            storyObject.GetComponent<StoryCell>().SetView(storiesList[i], OnStoryTapAction);
        }
    }

    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        storiesList.Clear();
    }

    void OnStoryTapAction(object storyId)
    {
        GameManager.Instance.apiHandler.GetStoryDetails((int)storyId, (status, response) => {

            Debug.Log("status = " + status);

            if (status)
            {
                storiesController.OnStoryButtonAction(response);
            }
        });
    }
}
