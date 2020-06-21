using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StoryDetailsView : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI ratingText;

    public TextMeshProUGUI genreText;




    public void SetView(DetailsScreenModel screenModel)
    {
        gameObject.SetActive(true);

        List<Genre> genres = DataManager.Instance.genres;

        Genre selectedGenre = genres.Find(item => item.id == screenModel.genreId);

        descriptionText.text = screenModel.description;

        genreText.text = selectedGenre.name;
    }
}
