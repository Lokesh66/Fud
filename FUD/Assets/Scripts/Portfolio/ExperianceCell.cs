using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        //ShowAlbums();
        OnButtonAction?.Invoke(experianceModel, false);
    }

    public void OnReadMoreButtonAction()
    {
        OnButtonAction?.Invoke(experianceModel, true);
    }

    void ShowAlbums()
    {
        /*List<MultimediaModel> expMedia = experianceModel.MultimediaModel;

        for (int i = 0; i < expMedia.Count; i++)
        {
            GameObject cellObject = Instantiate(mediaCell, content);

            cellObject.GetComponent<PortfolioMediaCell>().SetView(expMedia[i]);
        }*/
    }
}
