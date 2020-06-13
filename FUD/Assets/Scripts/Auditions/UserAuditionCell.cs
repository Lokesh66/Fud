using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class UserAuditionCell : MonoBehaviour
{
    public RawImage icon;
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
                EMediaType mediaType = DataManager.Instance.GetMediaType(auditionMultimedia.media_type);

                if (mediaType == EMediaType.Image)
                {
                    GameManager.Instance.downLoadManager.DownloadImage(auditionData.UserAuditionMultimedia[0].content_url, (sprite) =>
                    {
                        //icon.texture = sprite;
                    });
                }
            }
        }
    }

    public void SetVideoThumbnail(Action OnNext)
    {
        EMediaType mediaType = DataManager.Instance.GetMediaType(auditionMultimedia.media_type);

        if (mediaType == EMediaType.Video)
        {
            VideoStreamer.Instance.GetThumbnailImage(auditionData.UserAuditionMultimedia[0].content_url, (texture) =>
            {
                if (this != null)
                {
                    icon.texture = texture;

                    OnNext?.Invoke();
                }
            });
        }
        else {
            OnNext?.Invoke();
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
