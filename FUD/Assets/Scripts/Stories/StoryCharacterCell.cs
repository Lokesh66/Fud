using UnityEngine;
using TMPro;


public class StoryCharacterCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI suitPerformerText;

    public TextMeshProUGUI genderText;


    StoryCharacterModel characterModel;

    public void Load(StoryCharacterModel characterModel)
    {
        this.characterModel = characterModel;

        SetView();
    }

    void SetView() 
    {
        titleText.text = characterModel.title;

        descriptionText.text = characterModel.description;

        genderText.text = characterModel.gender;

        suitPerformerText.text = characterModel.suitable_performer;
    }
}
