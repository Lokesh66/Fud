using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesAlteredView : MonoBehaviour
{
    public RectTransform content;

    public GameObject alteredCell;

    public ETabType storiesTab;

    public StoriesAlteredPopUpView alteredPopUpView;

    public NoDataView noDataView;


    List<StoryAlteredModel> modelsList;


    public void EnableView()
    {
        ClearData();

        gameObject.SetActive(true);

        Load();
    }

    void Load()
    {
        GameManager.Instance.apiHandler.GetAlteredStories((status, modelsList) =>
        {
            if (status)
            {
                this.modelsList = modelsList;

                SetView();
            }
        });
    }

    void SetView()
    {
        content.DestroyChildrens();

        Debug.Log("modelsList Count = " + modelsList.Count);

        if (modelsList?.Count > 0)
        {
            for (int i = 0; i < modelsList.Count; i++)
            {
                GameObject storyObject = Instantiate(alteredCell, content);

                storyObject.GetComponent<StoryAlteredCell>().SetAlteredView(modelsList[i], OnAlteredTapAction);
            }
        }
        else
        {
            noDataView.SetView(GetNoDataModel());
        }
        noDataView.gameObject.SetActive(modelsList?.Count == 0);
    }

    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        modelsList?.Clear();
    }

    void OnAlteredTapAction(StoryAlteredModel alteredModel)
    {
        alteredPopUpView.Load(alteredModel);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Altered Stories Right Now";

        return noDataModel;
    }
}
