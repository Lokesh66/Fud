using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StoryCharactersView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;

    public NoDataView noDataView;


    List<StoryCharacterModel> characterModels;

    StoryDetailsController detailsController;


    public void Load(List<StoryCharacterModel> characterModels, StoryDetailsController storyDetails)
    {
        this.characterModels = characterModels;

        detailsController = storyDetails;

        gameObject.SetActive(true);

        if (characterModels?.Count > 0)
        {
            SetView();
        }
        else {
            noDataView.SetView(GetNoDataModel());
        }

        noDataView.gameObject.SetActive(characterModels?.Count == 0);
    }

    void SetView()
    {
        GameObject characterObject = null;

        for (int i = 0; i < characterModels.Count; i++)
        {
            characterObject = Instantiate(cellCache, content);

            characterObject.GetComponent<StoryCharacterCell>().Load(characterModels[i]);
        }
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Characters Right Now";

        noDataModel.buttonName = "Add Character";

        noDataModel.buttonAction = detailsController.OnAddButtonAction;

        return noDataModel;

    }

    void ClearData()
    {
        content.DestroyChildrens();
    }

    public void Refresh(StoryCharacterModel characterModel)
    {
        characterModels.Add(characterModel);

        gameObject.SetActive(true);

        GameObject characterObject = Instantiate(cellCache, content);

        characterObject.GetComponent<StoryCharacterCell>().Load(characterModel);

        noDataView.gameObject.SetActive(characterModels?.Count == 0);
    }
}
