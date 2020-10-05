using UnityEngine;
using TMPro;


public class AboutUsView : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;


    public void Load()
    {
        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {

    }

    public void OBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
