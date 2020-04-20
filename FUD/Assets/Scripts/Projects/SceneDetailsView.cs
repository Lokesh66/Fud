using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneDetailsView : MonoBehaviour
{

    SceneModel sceneModel;

    ProjectScenesPanel scenesPanel;


    public void Load(SceneModel sceneModel, ProjectScenesPanel scenesPanel)
    {
        this.sceneModel = sceneModel;

        this.scenesPanel = scenesPanel;

        gameObject.SetActive(true);
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        switch (buttonIndex)
        {
            case 0:
                OnEditButtonAction();
                break;
            case 1:
                OnViewButtonAction();
                break;
            case 2:
                OnDeleteButtonAction();
                break;
            case 3:
                OnCancelButtonAction();
                break;
        }
    }

    void OnCancelButtonAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        scenesPanel.OnEditButtonAction(sceneModel);
    }

    void OnViewButtonAction()
    {
        scenesPanel.OnViewButtonAction(sceneModel);
    }

    void OnDeleteButtonAction()
    {
        GameManager.Instance.apiHandler.RemoveProjectScene(sceneModel.id, 8, (status) =>
        {

            if (status)
            {
                scenesPanel.RemoveScene(sceneModel);
            }
        });
    }

    void Reset()
    {
        
    }
}
