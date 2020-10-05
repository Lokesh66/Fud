using UnityEngine;
using TMPro;


public class TermsAndConditionsView : MonoBehaviour
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
