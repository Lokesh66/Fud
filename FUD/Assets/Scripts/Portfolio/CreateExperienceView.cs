using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;

public class CreateExperienceView : MonoBehaviour
{
    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_Text startDateText;

    public TMP_Text endDateText;

    public TMP_InputField descriptionField;

    DateTime startDate;
    DateTime endDate;
    string dateDefaultText = "Select Date";

    string contentUrl = string.Empty;

    PortfolioView portfolioView = null;

    List<Genre> genres;

    List<IndustryModel> industryModels;

    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        startDate = DateTime.MinValue;
        endDate = DateTime.Now;

        LoadRoles();

        GameManager.Instance.apiHandler.GetIndustries((status, industriesList) => {

            if (status) 
            { 
                industryModels = industriesList;

                LoadIndustries();
            }
        });
           
    }

    void LoadRoles()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();
        roleDropDown.AddOptions(options);
    }

    void LoadIndustries()
    {
        List<string> options = new List<string>();

        foreach (var option in industryModels)
        {
            options.Add(option.name);
        }

        industryDropDown.ClearOptions();
        industryDropDown.AddOptions(options);
    }

    public void OnUploadButtonAction()
    {
        GalleryManager.Instance.PickImages(OnImagesUploaded);
    }

    public void CreateButtonAction()
    {
        string selectedGenreText = roleDropDown.options[roleDropDown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        string selectedIndustryText = industryDropDown.options[industryDropDown.value].text;

        IndustryModel selectedIndustry = industryModels.Find(industry => industry.name.Equals(selectedIndustryText));

        CreateExperianceModel experianceModel = new CreateExperianceModel();

        experianceModel.roleId = selectedGenre.id;

        experianceModel.description = descriptionField.text;

        experianceModel.industryId = selectedIndustry.id ;

        experianceModel.startDate = startDateText.text;

        experianceModel.endDate = endDateText.text;

        PortMultimediaModels multimediaModels = new PortMultimediaModels();

        PortMultiMediaModel mediaModel = new PortMultiMediaModel();

        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", "https://fud-user-1.s3.ap-south-1.amazonaws.com/04041934-32fa-4bcd-8946-692775222291.JPG");

        parameters[0].Add("media_type", "image");

        experianceModel.multimediaModels = parameters;

        GameManager.Instance.apiHandler.UpdateWorkExperiance(experianceModel, (status, response) => {

            if (status)
            {
                OnBackAction();
            }
        });
    }

    void OnImagesUploaded(bool status, List<string> imageURLs)
    {
        contentUrl = imageURLs[0];
    }

    public void OnBackAction()
    {
        portfolioView.OnRemoveLastSubView();

        Destroy(gameObject);
    }

    public void OnStartDateSelectedAction()
    {
        DatePicker.Instance.GetDate(startDate == DateTime.MinValue ? DateTime.Now : startDate, DateTime.MinValue, endDate, (date, value) =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                startDateText.text = value;
                startDate = date;
            }
            else
            {
                startDateText.text = dateDefaultText;
            }
        });
    }

    public void OnEndDateSelectedAction()
    {
        DatePicker.Instance.GetDate(endDate, startDate, DateTime.Now, (date, value) =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                endDateText.text = value;
                endDate = date;
            }
            else
            {
                endDateText.text = dateDefaultText;
            }
        });
    }
}

