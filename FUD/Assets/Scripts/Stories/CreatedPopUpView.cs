using UnityEngine;


public class CreatedPopUpView : MonoBehaviour
{
    public GameObject updateStoryCache;


    MyStoriesView storiesView;

    StoryCell storyCell;


    public void Load(StoryCell activityModel, MyStoriesView storiesView)
    {
        this.storyCell = activityModel;

        this.storiesView = storiesView;

        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        StoryDetailsController.Instance.Load(storyCell.GetStoryModel().id, OnBackFromDetailView);
    }

    void OnBackFromDetailView()
    {
        storiesView.OnStoryVersionShared();
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
