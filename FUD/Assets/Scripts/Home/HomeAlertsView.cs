using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeAlertsView : MonoBehaviour
{
    public RectTransform alertContent;

    public GameObject alertCell;


    List<HomeAlertModel> alertModels;


    public void Load()
    {
        GameManager.Instance.apiHandler.GetHomeAlerts(1, (status, response) =>
        {
            HomeAlertsResponse responseModel = JsonUtility.FromJson<HomeAlertsResponse>(response);

            if (status)
            {
                alertModels = responseModel.data;

                SetView();
            }
        });
    }

    void SetView()
    {
        GameObject cellObject = null;

        for (int i = 0; i < alertModels.Count; i++)
        {
            cellObject = Instantiate(alertCell, alertContent);

            cellObject.GetComponent<HomeAlertCell>().Load(alertModels[i]);
        }
    }
}
