using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProjectStoriesPanel : MonoBehaviour
{
    public RectTransform mediaContent;

    public GameObject mediaCell;

    public TMP_Text storyLineText;
    public TMP_Text descriptionText;
    public TMP_Text genreText;


    List<MultimediaModel> mediaList;


    public void SetData(StoryVersion storyVersion)
    {
        descriptionText.text = storyVersion.description;
        Genre genre = DataManager.Instance.genres.Find(item => item.id == storyVersion.genre_id);
        if (genre != null)
        {
            genreText.text = genre.name;
        }

        mediaList = storyVersion.Multimedia;

        LoadMedia();
    }

    void LoadMedia()
    {
        mediaContent.DestroyChildrens();

        VersionMediaCell _mediaCell = null;

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            _mediaCell = mediaObject.GetComponent<VersionMediaCell>();

            _mediaCell.SetView(mediaList[i]);
        }
    }
}
