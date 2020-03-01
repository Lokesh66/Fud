using UnityEngine;
using TMPro;
using System;

public class StoryCharacterCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI suitPerformerText;

    public TextMeshProUGUI genderText;


    StoryCharacterModel characterModel;

    Action<StoryCharacterModel> OnButtonAction;


    public void Load(StoryCharacterModel characterModel, Action<StoryCharacterModel> OnButtonAction)
    {
        this.characterModel = characterModel;

        this.OnButtonAction = OnButtonAction;

        SetView();
    }

    void SetView() 
    {
        titleText.text = characterModel.title;

        descriptionText.text = characterModel.description;

        genderText.text = characterModel.gender;

        suitPerformerText.text = characterModel.suitable_performer;
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(characterModel);
    }
}
