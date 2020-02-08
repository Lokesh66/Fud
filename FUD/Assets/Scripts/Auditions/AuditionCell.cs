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
        if (auditionData != null)
        {
            titleText.text = auditionData.topic;
            ageText.text = auditionData.age_to.ToString();
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
    }
}
