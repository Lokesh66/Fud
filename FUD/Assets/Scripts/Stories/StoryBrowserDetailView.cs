using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryBrowserDetailView : MonoBehaviour
{
    public RectTransform mediaContent;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI genreText;

    public GameObject mediaCell;


    StoryActivityModel model;

    List<MultimediaModel> mediaList;

    List<VersionMediaCell> cellsList = new List<VersionMediaCell>();


    public void Load(StoryActivityModel model)
    {
        this.model = model;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        descriptionText.text = model.StoryVersions.description;

        Genre genre = DataManager.Instance.genres.Find(genreItem => genreItem.id == model.StoryVersions.genre_id);

        genreText.text = genre.name;

        mediaList = model.StoryVersions.Multimedia;

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

    public void OnBackButtonAction()
    {
        ResetData();

        gameObject.SetActive(false);
    }

    void ResetData()
    {
        genreText.text = descriptionText.text = string.Empty;
    }
}
