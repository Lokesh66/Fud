using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectCastCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;
    public TMP_Text descriptionText;

    ProjectCast castData;
    int index = 0;

    public void SetView(int index, ProjectCast cast)
    {
        castData = cast;
        this.index = index;
        if (castData != null)
        {
            titleText.text = "";// castData.;
            ageText.text = "";// castData.age_to.ToString();
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
    }
}
