using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ExperianceCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI locationText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI timeText;

    public GameObject mediaCell;


    WorkExperianceModel experianceModel;

    Action<WorkExperianceModel> OnButtonAction;

    public void SetView(WorkExperianceModel experianceModel, Action<WorkExperianceModel> OnButtonAction)
    {
        this.experianceModel = experianceModel;

        this.OnButtonAction = OnButtonAction;

        descriptionText.text = experianceModel.description;
    }

    public void OnTapAction()
    {
        //ShowAlbums();
        OnButtonAction?.Invoke(experianceModel);
    }

    void ShowAlbums()
    {
        /*List<WorkExpMedia> expMedia = experianceModel.WorkExpMedia;

        for (int i = 0; i < expMedia.Count; i++)
        {
            GameObject cellObject = Instantiate(mediaCell, content);

            cellObject.GetComponent<PortfolioMediaCell>().SetView(expMedia[i]);
        }*/
    }
}
