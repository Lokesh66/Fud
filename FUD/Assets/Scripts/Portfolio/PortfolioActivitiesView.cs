using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioActivitiesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject activityCell;

    public GameObject noDataObject;

    public ETabType tabType;

    public PortfolioActivityPopUp activityPopUp;


    List<PortfolioActivityModel> activityModels;


    public void Load()
    {
        if (tabType == ETabType.Offers)
        {
            LoadOfferedData();
        }
        else {
            LoadAlteredData();
        }

        gameObject.SetActive(true);
    }

    void LoadOfferedData()
    {
        GameManager.Instance.apiHandler.GetPortfolioPosts((status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            Debug.Log("LoadOfferedData : Response = " + response);

            if (status)
            {
                activityModels = responseModel.data;

                noDataObject.SetActive(activityModels.Count == 0);

                SetView();
            }
        });
    }

    void LoadAlteredData()
    {
        GameManager.Instance.apiHandler.GetAlteredPortfolios((status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            Debug.Log("LoadAlteredData : Response = " + response);

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
 
            cellObject.GetComponent<PortfolioActvityCell>().Load(activityModels[i], activityPopUp, tabType);
        }
    }
}
