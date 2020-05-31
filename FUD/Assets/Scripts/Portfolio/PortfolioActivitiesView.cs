using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioActivitiesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject activityCell;

    public GameObject noDataObject;

    public PortfolioActivityPopUp activityPopUp;

    List<PortfolioActivityModel> activityModels;

    public void EnableView()
    {
        Load();

        gameObject.SetActive(true);
    }

    void Load()
    {
        GameManager.Instance.apiHandler.GetPortfolioPosts((status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            Debug.Log("Response = " + response);

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

            cellObject.GetComponent<PortfolioActvityCell>().Load(activityModels[i], activityPopUp);
        }
    }
}
