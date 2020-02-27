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
        GalleryManager.Instance.PickImages(OnImagesUploaded);
    }

    public void CreateButtonAction()
    {
        PortMultimediaModels multimediaModels = new PortMultimediaModels();

        PortMultiMediaModel mediaModel = new PortMultiMediaModel();

        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", contentUrl);

        parameters[0].Add("media_type", "image");

        GameManager.Instance.apiHandler.CreatePortfolio(titleField.text, descriptionField.text, parameters, (status, response) => {

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
