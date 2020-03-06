using UnityEngine;
using System.Collections.Generic;

public class ProjectScenesPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject sceneCell;
    public GameObject addNewButton;
    public NoDataView noData;

    Project projectModel;

    private void OnEnable()
    {
        addNewButton.SetActive(true);
    }

    private void OnDisable()
    {
        addNewButton.SetActive(false);
    }

    public void SetData(Project projectModel, List<SceneModel> sceneModels)
    {
        this.projectModel = projectModel;

        parentContent.DestroyChildrens();

        if (sceneModels != null && sceneModels.Count > 0)
        {
            noData.gameObject.SetActive(false);

            for (int i = 0; i < sceneModels.Count; i++)
            {
                GameObject sceneObject = Instantiate(sceneCell, parentContent);

                sceneObject.GetComponent<ProjectSceneCell>().SetView(i, sceneModels[i]);
            }
        }
        else
        {
            EnableNodata();
        }

    }
    public void CreateScene()
    {
        CreateScenesView.Instance.SetView(projectModel, (isNewDataUpdated) => {
            if (isNewDataUpdated)
            {
                ProjectsDetailedView.Instance.Reload();
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
        model.buttonName = "Create Scene";
        model.subTitle = "No Scenes Right now";
        model.buttonAction = CreateScene;
        return model;
    }
}
