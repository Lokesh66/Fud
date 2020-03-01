using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UpdatePortfolioView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField descriptionField;


    PortfolioModel portfolioModel;

    string contentUrl = string.Empty;

    public void Load(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        this.portfolioModel = portfolioModel;

        SetView();
    }

    void SetView()
    {
        titleField.text = portfolioModel.title;

        descriptionField.text = portfolioModel.description;
    }

    public void OnUploadButtonAction()
    {
        GalleryManager.Instance.PickImages(OnImagesUploaded);
    }

    void OnImagesUploaded(bool status, List<string> imageURLs)
    {
        contentUrl = imageURLs[0];
    }

    public void OnSaveButtonAction()
    {
        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        parameters.Add(new Dictionary<string, object>());

        parameters[0].Add("content_id", 1);

        parameters[0].Add("content_url", contentUrl);

        parameters[0].Add("media_type", "image");

        GameManager.Instance.apiHandler.UpdatePortfolio(titleField.text, descriptionField.text, portfolioModel.id, parameters, (status, response) => {

            if (status)
            {
                OnBackButtonAction();
            }
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
