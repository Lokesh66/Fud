using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;
/*    public TMP_Text otherText;
*/
    Audition auditionData;
    int index = 0;

    public void SetView(int index, Audition audition)
    {
        auditionData = audition;
        this.index = index;
        /*titleText.text = "";
        ageText.text = "";*/
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
    }
}
