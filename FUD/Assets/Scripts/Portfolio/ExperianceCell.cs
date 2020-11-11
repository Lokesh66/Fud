using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class ExperianceCell : MonoBehaviour
{
    public TextMeshProUGUI roleText;

    public TextMeshProUGUI locationText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI timeText;

    public GameObject mediaCell;


    WorkExperianceModel experianceModel;

    Action<WorkExperianceModel, bool> OnButtonAction;

    public void Load(WorkExperianceModel experianceModel, Action<WorkExperianceModel, bool> OnButtonAction)
    {
        this.experianceModel = experianceModel;

        this.OnButtonAction = OnButtonAction;

        SetView();
    }

    void SetView()
    {
        List<Craft> genres = DataManager.Instance.crafts;

        Craft genre = genres.Find(item => item.id == experianceModel.role_id);

        roleText.text = genre.name;

        locationText.text = experianceModel.MasterData.name;

        descriptionText.text = experianceModel.description;
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(experianceModel, false);
    }

    public void OnReadMoreButtonAction()
    {
        OnButtonAction?.Invoke(experianceModel, true);
    }
}
