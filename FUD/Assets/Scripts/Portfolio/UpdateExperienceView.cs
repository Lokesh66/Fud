using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class UpdateExperienceView : MonoBehaviour
{
    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_InputField startDate;

    public TMP_InputField endDate;

    public TMP_InputField descriptionField;


    string contentUrl = string.Empty;

    WorkExperianceModel workModel = null;

    List<Genre> genres;


    List<IndustryModel> industryModels;


    public void Load(WorkExperianceModel workModel)
    {
        this.workModel = workModel;

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

        Genre requiredGenre = genres.Find(genre => genre.id == workModel.id);

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(requiredGenre.name));

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();

        roleDropDown.AddOptions(options);

        roleDropDown.value = roleDropDown.options.FindIndex(option => options.Equals(selectedGenre.name));
    }

    void LoadIndustries()
    {
        IndustryModel requiredGenre = industryModels.Find(industryModel => industryModel.id == workModel.id);

        IndustryModel selectedGenre = industryModels.Find(industryModel => industryModel.name.Equals(requiredGenre.name));

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        industryDropDown.ClearOptions();

        industryDropDown.AddOptions(options);

        industryDropDown.value = industryDropDown.options.FindIndex(option => options.Equals(selectedGenre.name));
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

        experianceModel.startDate = string.Format("{0:yyyy/MM/dd} {1:hh:mm:ss}", System.DateTime.Now, System.DateTime.Now);

        experianceModel.endDate = string.Format("{0:yyyy/MM/dd} {1:hh:mm:ss}", System.DateTime.Now, System.DateTime.Now);

        PortMultimediaModels multimediaModels = new PortMultimediaModels();

        PortMultiMediaModel mediaModel = new PortMultiMediaModel();

        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", "https://fud-user-1.s3.ap-south-1.amazonaws.com/15572033-40e9-47c2-8532-9a49d7206e6c.png");

        parameters[0].Add("media_type", "image");

        experianceModel.multimediaModels = parameters;

        GameManager.Instance.apiHandler.UpdateWorkExperiance(experianceModel, (status, response) => {

            if (status)
            {
                OnBackAction();
            }
        });
    }

    public void OnBackAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    void OnImagesUploaded(bool status, List<string> imageURLs)
    {
        contentUrl = imageURLs[0];
    }

    void ClearData()
    {
        industryDropDown.ClearOptions();

        roleDropDown.ClearOptions();
    }
}

