using System.Collections.Generic;
using UnityEngine;

public class ProjectCastsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject castCell;
    public NoDataView noData;

    public void SetData(int projectId, List<ProjectCast> casts)
    {
        for (int i = 0; i < casts.Count; i++)
        {
            GameObject auditionObject = Instantiate(castCell, parentContent);

            auditionObject.GetComponent<ProjectCastCell>().SetView(i, casts[i]);
        }

        GameManager.Instance.apiHandler.GetProjectCharacters(projectId, (status, response) => {

        });
    }
    public void CreateCast()
    {
        CreateCastView.Instance.SetView(1, (isNewDataUpdated) =>
        {
            if (isNewDataUpdated)
            {
            }
        });
    }

    void EnableNodata()
    {
        noData.gameObject.SetActive(true);
        noData.SetView(GetNoDataModel());
    }
    NoDataModel GetNoDataModel()
    {
        NoDataModel model = new NoDataModel();
        model.buttonName = "Create Cast";
        model.subTitle = "No Cast Right now";
        model.buttonAction = CreateCast;
        return model;
    }
}
