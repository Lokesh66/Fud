using UnityEngine;
using TMPro;


public class CharacterView : MonoBehaviour
{
    public TMP_Text castText;

    public TMP_Text titleText;

    public TMP_Text characterText;

    public TMP_Text genderText;


    public TMP_Text descriptionText;

    public RectTransform mediaContent;

    public GameObject mediaCell;


    StoryCharacterModel characterModel;

    public void Load(StoryCharacterModel characterModel)
    {
        this.characterModel = characterModel;

        SetView();

        gameObject.SetActive(true);
    }

    void SetView()
    {
        titleText.text = characterModel.title;

        castText.text = characterModel.cast_name;

        descriptionText.text = characterModel.description;

        genderText.text = characterModel.gender;

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.GetOtherUserInfo(characterModel.id, storyId, (status, response) => {

            if (status)
            {
                PerformerResponse reponseModel = JsonUtility.FromJson<PerformerResponse>(response);

                characterText.text = reponseModel.data.UserInfo.name;
            }
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
