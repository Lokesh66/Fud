using UnityEngine;
using System;
using TMPro;

public class ProjectSceneCell : MonoBehaviour
{
    public TMP_Text sceneOrderText;
    public TMP_Text shootTimeText;
    public TMP_Text placeText;
    public TMP_Text locationText;

    SceneModel sceneModel;

    public int index = 0;

    public Action<SceneModel> OnCellButtonAction;

    public void SetView(int index, SceneModel sceneModel, Action<SceneModel> OnCellButtonAction)
    {
        this.sceneModel = sceneModel;
        this.index = index;

        this.OnCellButtonAction = OnCellButtonAction;

        if (this.sceneModel != null)
        {
            sceneOrderText.text = "Scene order " + this.sceneModel.scene_order.ToString();
            placeText.text = this.sceneModel.place_type;
            shootTimeText.text = this.sceneModel.shoot_time;
            locationText.text = this.sceneModel.location;
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
        OnCellButtonAction?.Invoke(sceneModel);
    }
}
