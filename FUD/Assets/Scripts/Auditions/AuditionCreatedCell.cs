using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AuditionCreatedCell : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text peopleJoinedText;
    public TMP_Text projectNameText;

    Audition auditionData;
    AuditionCreatedView auditionController;


    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();


    public void SetView(AuditionCreatedView auditionController, Audition audition)
    {
        auditionData = audition;
        this.auditionController = auditionController;

        if (auditionData != null)
        {
            titleText.text = auditionData.topic;

            descriptionText.text = auditionData.description;

            peopleJoinedText.text = "Entries : " + auditionData.no_of_persons_joined.ToString();

            projectNameText.text = auditionData.title;
        }
    }

    public void OnClickAction()
    {
        auditionController.OnCellTapAction(this);
    }

    public Audition GetAuditionModel()
    {
        return auditionData;
    }
}
