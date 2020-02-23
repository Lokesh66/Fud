using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class UpdateExperianceView : MonoBehaviour
{
    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_InputField startDate;

    public TMP_InputField endDate;

    public TMP_InputField descriptionField;


    string contentUrl = string.Empty;

    PortfolioView portfolioView = null;

    List<Genre> genres;

    List<IndustryModel> industryModels;


    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

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

    public void Init(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;
    }

    public void OnUploadButtonAction()
    {
        PickImages(SystemInfo.maxTextureSize);
    }

    public void CreateButtonAction()
    {
        string selectedGenreText = roleDropDown.options[roleDropDown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        string selectedIndustryText = industryDropDown.options[industryDropDown.value].text;

        IndustryModel selectedIndustry = industryModels.Find(industry => industry.name.Equals(selectedIndustryText));

        CreateExperianceModel experianceModel = new CreateExperianceModel();

        experianceModel.roleId = selectedGenre.id;

        experianceModel.industryId = selectedIndustry.id ;

        experianceModel.startDate = System.DateTime.Now;

        experianceModel.endDate = System.DateTime.Now;

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

    private void PickImages(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);

            if (path != null)
            {
                // Create Texture from selected image

                Texture2D texture = NativeGallery.LoadImageAtPath(path[0], maxSize, true, true, false, UploadFile);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //NativeGallery.GetImageProperties(path);

                //screenShotImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);

                //screenShotImage.SetNativeSize();

                byte[] textureBytes = texture.EncodeToPNG();

                string filePath = APIConstants.IMAGES_PATH + "/GalleryPhotos";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.WriteAllBytes(filePath, textureBytes);
            }
        }, "Select a PNG image");

        Debug.Log("Permission result: " + permission);
    }

    void UploadFile(string filePath)
    {
        GameManager.Instance.apiHandler.UploadFile(filePath, EMediaType.Image, (status, response) => {

            FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

            contentUrl = responseModel.data.s3_file_path;
        });
    }

    public void OnBackAction()
    {
        portfolioView.OnRemoveLastSubView();

        Destroy(gameObject);
    }
}

