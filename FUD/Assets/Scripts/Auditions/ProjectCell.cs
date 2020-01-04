using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ProjectCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public Image projectIcon;


    public void SetView()
    { 
    
    }

    public void OnButtonAction()
    { 
        //Need to call API for list of auditions for this projectby sending Project Id
    }
}
