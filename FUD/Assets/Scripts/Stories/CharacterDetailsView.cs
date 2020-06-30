using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterDetailsView : MonoBehaviour
{
    //public Image userImage;

    public TextMeshProUGUI description;

    public UpdateCharacterView updateCharacterView;

    public CharacterView characterView;


    StoryCharacterModel characterModel;

    StoryCharactersView charactersView;


    public void Load(StoryCharacterModel characterModel, StoryCharactersView charactersView)
    {
        this.characterModel = characterModel;

        this.charactersView = charactersView;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        //description.text = characterModel.description;
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        switch (buttonIndex)
        {
            case 0:
                OnEditButtonAction();
                break;
            case 1:
                OnDeleteButtonAction();
                break;
            case 2:
                OnCancelButtonAction();
                break;
            case 3:
                OnViewButtonAction();
                break;
        }
    }

    void OnViewButtonAction()
    {
        characterView.Load(characterModel);
    }

    void OnCancelButtonAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        updateCharacterView.Load(characterModel, OnUpdateSuccess);
    }

    void OnUpdateSuccess(StoryCharacterModel characterModel)
    {
        charactersView.UpdateModel(characterModel);
    }

    void OnDeleteButtonAction()
    {
        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.RemoveCharacter(characterModel.id, storyId, 8, (status) => {

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Character Removed Successfully" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        charactersView.OnRemoveCharacter(characterModel);
    }

    void Reset()
    {
        //description.text = string.Empty;

        //userImage.sprite = null;
    }
}
