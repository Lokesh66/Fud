using UnityEngine;
using TMPro;

public class ProjectSceneCell : MonoBehaviour
{
    public TMP_Text sceneOrderText;
    public TMP_Text shootTimeText;
    public TMP_Text placeText;
    public TMP_Text locationText;

    SceneModel sceneModel;
    int index = 0;

    public void SetView(int index, SceneModel sceneModel)
    {
        this.sceneModel = sceneModel;
        this.index = index;

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
    }
}
