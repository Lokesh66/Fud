
using System.Collections.Generic;
using UnityEngine;


public class MyStoriesView : BaseView
{
    public RectTransform content;

    public GameObject storyCell;


    List<Story> storiesList;

    protected override void EnableView()
    {
        base.EnableView();

       // Load();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    protected override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();
    }


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

    void SetView()
    {
        for (int i = 0; i < storiesList.Count; i++)
        {
            GameObject storyObject = Instantiate(storyCell, content);

            storyObject.GetComponent<StoryCell>().SetView(storiesList[i]);
        }
    }

    public void OnBackButtonAction()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);
    }
}
