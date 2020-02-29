using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VersionMultimediaView : MonoBehaviour
{
    public RectTransform mediaContent;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI genreText;

    public GameObject mediaCell;

    StoryVersionDetailModel storyVersion;

    List<MultimediaModel> mediaList;


    public void Load(StoryVersion storyVersion)
    {
        GetVersionDetails(storyVersion.id);
    }

    void GetVersionDetails(int versionId)
    {
        GameManager.Instance.apiHandler.GetStoryVersionDetails(versionId, (status, response) => {

            MultiMediaResponse responseModel = JsonUtility.FromJson<MultiMediaResponse>(response);

            if (status)
            {
                gameObject.SetActive(true);

                storyVersion = responseModel.data[0];

                mediaList = storyVersion.Multimedia;

                SetView();
            }
        });
    }

    void SetView()
    {
        descriptionText.text = storyVersion.description;

        Genre genre = DataManager.Instance.genres.Find(genreItem => genreItem.id == storyVersion.genre_id);

        genreText.text = genre.name;

        LoadMedia();
    }

    void LoadMedia()
    {
        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            mediaObject.GetComponent<VersionMediaCell>().SetView(mediaList[i]);
        }
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
