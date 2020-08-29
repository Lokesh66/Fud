using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedPopUpView : MonoBehaviour
{
    public GameObject updateStoryCache;


    StoryCell storyCell;


    public void Load(StoryCell activityModel)
    {
        this.storyCell = activityModel;

        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        StoryDetailsController.Instance.Load(storyCell.GetStoryModel().id, null);
    }

    public void OnEditButtonAction()
    {
        gameObject.SetActive(false);

        Transform parent = StoryDetailsController.Instance.transform;

        GameObject createObject = Instantiate(updateStoryCache, parent);

        createObject.GetComponent<StoryUpdateView>().Load(storyCell.GetStoryModel(), storyCell);
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }
}
