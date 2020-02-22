using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryActivitiesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject activityCell;

    List<StoryActivityModel> activityModels;

    public void EnableView()
    {
        Load();

        gameObject.SetActive(true);
    }

    void Load()
    { 
        GameManager.Instance.apiHandler.GetStoryPosts((status, response) => {

            StoryActivityResponseModel responseModel = JsonUtility.FromJson<StoryActivityResponseModel>(response);

            activityModels = responseModel.data;

            SetView();
        });
    }

    void SetView()
    {
        int totalCount = activityModels.Count;

        content.DestroyChildrens();

        for (int i = 0; i < totalCount; i++)
        {
            GameObject cellObject = Instantiate(activityCell, content);

            cellObject.GetComponent<StoryActivityCell>().Load(activityModels[i]);
        }
    }
}
