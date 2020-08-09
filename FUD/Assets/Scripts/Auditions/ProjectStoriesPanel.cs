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

    List<VersionMediaCell> cellsList = new List<VersionMediaCell>();


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

        cellsList.Clear();

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            _mediaCell = mediaObject.GetComponent<VersionMediaCell>();

            _mediaCell.SetView(mediaList[i]);

            cellsList.Add(_mediaCell);
        }

        if (mediaList.Count > 0)
        {
            SetVideoThumbnails(0);
        }
    }

    void SetVideoThumbnails(int index)
    {
        cellsList[index].SetVideoThumbnail(() => {

            index++;

            if (index >= mediaList.Count)
            {
                return;
            }

            SetVideoThumbnails(index);
        });
    }
}
