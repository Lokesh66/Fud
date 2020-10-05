using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class HomeAlertsView : MonoBehaviour
{
    public RectTransform alertContent;

    public RectTransform scrollRectTrans;

    public HorizontalLayoutGroup layoutGroup;

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

                alertContent.sizeDelta = new Vector2((scrollRectTrans.rect.width + layoutGroup.spacing) * alertModels.Count, alertContent.sizeDelta.y);

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

            cellObject.GetComponent<HomeAlertCell>().Load(alertModels[i], scrollRectTrans.rect.width);
        }
    }
}
