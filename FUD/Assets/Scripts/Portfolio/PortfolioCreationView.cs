using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class PortfolioCreationView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField descriptionField;
    

    string contentUrl = string.Empty;

    PortfolioView portfolioView = null;
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
        PortMultimediaModels multimediaModels = new PortMultimediaModels();

        PortMultiMediaModel mediaModel = new PortMultiMediaModel();

        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", "https://fud-user-1.s3.ap-south-1.amazonaws.com/15572033-40e9-47c2-8532-9a49d7206e6c.png");

        parameters[0].Add("media_type", "image");

        GameManager.Instance.apiHandler.CreatePortfolio(titleField.text, descriptionField.text, parameters, (status, response) => {

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
        GameManager.Instance.apiHandler.UploadFile(filePath, (status, response) => {

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

public class FileUploadModel
{
    public bool success;
    public string s3_file_path;
}

public class FileUploadResponseModel : BaseResponse
{
    public FileUploadModel data;
}
