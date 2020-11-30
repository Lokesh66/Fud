using UnityEngine;
using TMPro;
using System;

public class StoryCharacterCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI suitPerformerText;

    public TextMeshProUGUI genderText;

    public GameObject readMoreObject;


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
        Craft selectedCraft = DataManager.Instance.crafts.Find(item => item.id.Equals(characterModel.role_id));

        titleText.text = characterModel.title;

        descriptionText.text = characterModel.description;

        genderText.text = selectedCraft.name;

        suitPerformerText.text = characterModel.suitable_performer;

        Debug.Log("SetView : Preferred Height : " + descriptionText.preferredHeight);
    }

    public void OnReadMoreButtonAction()
    {
        descriptionText.overflowMode = TextOverflowModes.Overflow;

        readMoreObject.SetActive(false);

        Debug.Log("OnReadMoreButtonAction : Preferred Height : " + descriptionText.preferredHeight);

        Vector2 cellSize = rectTransform.sizeDelta;

        cellSize.y += descriptionText.preferredHeight - 120.0f;

        rectTransform.sizeDelta = new Vector2(cellSize.x, cellSize.y);
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(characterModel);
    }
}
