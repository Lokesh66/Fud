using UnityEngine;
using System.Collections.Generic;

public class ProjectScenesPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject sceneCell;
    public GameObject addNewButton;
    public NoDataView noData;

    public SceneDetailsView sceneDetails;

    public ProjectSceneView sceneView;

    public UpdateSceneView updateSceneView;


    Project projectModel;

    List<SceneModel> sceneModels;

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

        this.sceneModels = sceneModels;

        parentContent.DestroyChildrens();

        if (sceneModels != null && sceneModels.Count > 0)
        {
            noData.gameObject.SetActive(false);

            for (int i = 0; i < sceneModels.Count; i++)
            {
                GameObject sceneObject = Instantiate(sceneCell, parentContent);

                sceneObject.GetComponent<ProjectSceneCell>().SetView(i, sceneModels[i], OnCellButtonAction);
            }
        }
        else
        {
            EnableNodata();
        }
    }

    void OnCellButtonAction(SceneModel sceneModel)
    {
        sceneDetails.Load(sceneModel, this);
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

    public void RemoveScene(SceneModel sceneModel)
    {
        int modelIndex = sceneModels.IndexOf(sceneModel);

        Destroy(parentContent.GetChild(modelIndex).gameObject);

        sceneModels.Remove(sceneModel);

        if (sceneModels.Count <= 0)
        {
            EnableNodata();
        }
    }

    public void OnEditButtonAction(SceneModel sceneModel)
    {
        updateSceneView.Load(sceneModel, this);
    }

    public void OnViewButtonAction(SceneModel sceneModel)
    {
        sceneView.Load(sceneModel, this);
    }
}
