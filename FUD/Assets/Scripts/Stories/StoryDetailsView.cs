using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class StoryDetailsView : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI ratingText;

    public TextMeshProUGUI reviewsNoText;


    public MyStoriesView storiesView;



    public void SetView(DetailsScreenModel screenModel)
    {
        gameObject.SetActive(true);

        descriptionText.text = screenModel.description;

        ratingText.text = screenModel.rating.ToString();
    }
}
