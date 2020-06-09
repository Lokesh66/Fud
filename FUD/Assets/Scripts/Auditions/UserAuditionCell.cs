using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UserAuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;

    SearchAudition auditionData;

    MultimediaModel auditionMultimedia;

    Action<SearchAudition> OnSelect;

    public void SetView(SearchAudition audition, Action<SearchAudition> action)
    {
        auditionData = audition;
        OnSelect = action;

        if (auditionData != null)
        {
            titleText.text = auditionData.status;
            ageText.text = auditionData.user_id.ToString();

            auditionMultimedia = auditionData.UserAuditionMultimedia[0];

            if (auditionData.UserAuditionMultimedia != null && auditionData.UserAuditionMultimedia.Count > 0)
            {
                if (auditionMultimedia.GetMediaType(auditionMultimedia.media_type) == EMediaType.Image)
                {
                    GameManager.Instance.downLoadManager.DownloadImage(auditionData.UserAuditionMultimedia[0].content_url, (sprite) =>
                    {
                        icon.sprite = sprite;
                    });
                }
                else if (auditionMultimedia.GetMediaType(auditionMultimedia.media_type) == EMediaType.Video)
                { 
                    //Show the first frame of a video
                }
            }
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
        OnSelect?.Invoke(auditionData);
        /*AuditionJoinView.Instance.Load(auditionData, false, (index) => {
            switch (index)
            {
                case 3:
                    break;
                case 4:
                    break;
            }
        });*/
    }

}
