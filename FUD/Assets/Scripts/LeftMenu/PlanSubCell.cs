using UnityEngine;
using TMPro;

public class PlanSubCell : MonoBehaviour
{
    public TextMeshProUGUI moduleNameText;

    public TextMeshProUGUI countText;


    Feature featureModel;
    public void Load(Feature feature)
    {
        featureModel = feature;

        SetView();
    }

    void SetView()
    {
        moduleNameText.text = featureModel.name;

        countText.text = featureModel.count.ToString();
    }
}
