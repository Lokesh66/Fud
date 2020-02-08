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
}
