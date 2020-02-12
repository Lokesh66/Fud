using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UpdateExperianceView : MonoBehaviour
{
    public TMP_InputField descriptionField;

    public TMP_Dropdown dropdown;


    private string contentUrl;

    List<Craft> craftsList;

    PortfolioView portfolioView;


    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        craftsList = DataManager.Instance.crafts;

        List<string> options = new List<string>();

        foreach (var option in craftsList)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void OnUploadAction()
    {
        PickImages(SystemInfo.maxTextureSize);

        //GetAudioFromGallery();
    }

    public void OnSubmitAction()
    {
        string selectedGenreText = dropdown.options[dropdown.value].text;

        Craft selectedGenre = craftsList.Find(genre => genre.name.Equals(selectedGenreText));

        PortMultimediaModels multimediaModels = new PortMultimediaModels();

        PortMultiMediaModel mediaModel = new PortMultiMediaModel();

        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", contentUrl);

        parameters[0].Add("media_type", "image");

        GameManager.Instance.apiHandler.UpdateWorkExperiance(descriptionField.text, selectedGenre.id, parameters, (status, response) => {

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
        GameManager.Instance.apiHandler.UploadFile(filePath, (status, response) => {

            FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

            contentUrl = responseModel.data.s3_file_path;
        });
    }

    public void OnBackAction()
    {
        portfolioView.OnRemoveLastSubView();
    }
}
