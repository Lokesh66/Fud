using System;
using TMPro;
using UnityEngine;

public class StoryPublicPopUp : MonoBehaviour
{
    //public Image storyImage;

    public GameObject acceptObject;

    public MyStoriesBrowseView browsersView;

    public StoryBrowserDetailView detailView;


    StoryModel storyModel;


    Action<int> OnClose;


    public void Load(StoryModel storyModel, Action<int> OnClose)
    {
        gameObject.SetActive(true);

        this.storyModel = storyModel;

        this.OnClose = OnClose;

        //descriptionText.text = activityModel.comment;
    }

    public void OnStatusUpdate(int statusIndex)
    {
        if (statusIndex != -1)
        {
            GameManager.Instance.apiHandler.UpdateStoryPostStatus(storyModel.id, statusIndex, (status, message) =>
            {
                if (status)
                {
                    //browsersView.OnStatusApplied(activityModel);
                    OnClose?.Invoke(statusIndex);
                    gameObject.SetActive(false);
                }
            });
        }
        else {
            OnClose?.Invoke(statusIndex);
            gameObject.SetActive(false);
        }
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetPublicStoryDetails(storyModel.id, (status, response) => {

            if (status)
            {
                StoryBrowseDetailResponse responseModel = JsonUtility.FromJson<StoryBrowseDetailResponse>(response);

                if (responseModel.data != null)
                {
                    
                }
            }
        });
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }
}
