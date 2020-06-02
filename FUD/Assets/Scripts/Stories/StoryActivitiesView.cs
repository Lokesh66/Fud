using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryActivitiesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject activityCell;

    public GameObject noDataObject;

    public StoryActivityPopUp activityPopUp;


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

            if (status)
            {
                activityModels = responseModel.data;

                noDataObject.SetActive(activityModels.Count == 0);

                SetView();
            }
        });
    }

    void SetView()
    {
        int totalCount = activityModels.Count;

        content.DestroyChildrens();

        for (int i = 0; i < totalCount; i++)
        {
            GameObject cellObject = Instantiate(activityCell, content);

            cellObject.GetComponent<StoryActivityCell>().Load(activityModels[i], activityPopUp);
        }
    }

    public void OnStatusApplied(StoryActivityModel activityModel)
    {
        activityModels.Remove(activityModel);

        noDataObject.SetActive(activityModels.Count == 0);
    }
}
