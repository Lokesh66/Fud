using UnityEngine;
using TMPro;

public class ProjectStoriesPanel : MonoBehaviour
{
    public TMP_Text storyLineText;
    public TMP_Text descriptionText;
    public TMP_Text genreText;
    public TMP_Text typeText;
    public TMP_Text versionsText;

    public GameObject moreStoriesPanel;
    public TMP_Text moreStoriesText;

    public Transform parentContent;
    public GameObject storyCell;

    public void SetData(StoryVersion storyVersion)
    {
        descriptionText.text = storyVersion.description;
        Genre genre = DataManager.Instance.genres.Find(item => item.id == storyVersion.genre_id);
        if (genre != null)
        {
            genreText.text = genre.name;
        }
    }
}
