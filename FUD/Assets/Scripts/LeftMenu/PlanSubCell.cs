using UnityEngine;
using TMPro;

public class PlanSubCell : MonoBehaviour
{
    public TextMeshProUGUI moduleNameText;

    public TextMeshProUGUI countText;


    public void Load(object model, bool isPlanActive)
    {
        SetView(model, isPlanActive);
    }

    void SetView(object model, bool isPlanActive)
    {
        Feature feature = null;

        FeaturedModel featuredModel = null;

        if (isPlanActive)
        {
            featuredModel = model as FeaturedModel;
        }
        else {
            feature = model as Feature;
        }
        
        moduleNameText.text = isPlanActive ? featuredModel.name : feature.name;

        string countMessage = isPlanActive ? featuredModel.used_count.ToString("00") + "/" + featuredModel.total_count.ToString("00") : feature.count.ToString();

        countText.text = countMessage;
    }
}
