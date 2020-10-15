using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UMP;


public class UserAuditionCell : MonoBehaviour
{
    public UniversalMediaPlayer mediaPlayer;

    public RawImage icon;

    public TMP_Text titleText;
    public TMP_Text ageText;

    public Texture2D defaultTexture;

    [HideInInspector]
    public SearchAudition auditionData;

    MultimediaModel auditionMultimedia;

    Action<UserAuditionCell> OnSelect;

    EMediaType mediaType;


    public void SetView(SearchAudition audition, Action<UserAuditionCell> action)
    {
        auditionData = audition;
        OnSelect = action;

        if (auditionData != null)
        {
            titleText.text = auditionData.status;
            ageText.text = auditionData.Users.name;

            List<MultimediaModel> modelsList = auditionData.UserAuditionMultimedia;

            if (modelsList != null && modelsList.Count > 0)
            {
                MultimediaModel model = modelsList.Find(item => item.GetMediaType(item.media_type) == EMediaType.Image);

                if (model != null)
                {
                    this.mediaType = EMediaType.Image;

                    auditionData.onScreenModel = model;
                }
                else {
                    EMediaType mediaType = modelsList[0].GetMediaType(modelsList[0].media_type);

                    if (mediaType == EMediaType.Video || mediaType == EMediaType.Audio)
                    {
                        mediaPlayer.Path = modelsList[0].content_url;

                        mediaPlayer.Prepare();

                        mediaPlayer.AddEndReachedEvent(() =>
                        {
                            UpdateAuditionStatus();
                        });

                        if (mediaType == EMediaType.Video)
                        {
                            mediaPlayer.AddImageReadyEvent((texture) =>
                            {
                                icon.texture = texture;
                            });
                        }
                    }
                }
            }
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
        OnSelect?.Invoke(this);
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

    public void OnPlayButtonAction()
    {
        if (mediaType == EMediaType.Video || mediaType == EMediaType.Audio)
        {
            UIManager.Instance.topCanvas.PlayVideo(icon, mediaPlayer, mediaType);
        }
    }

    void UpdateAuditionStatus()
    {
        GameManager.Instance.apiHandler.UpdateAuditionStatus(auditionData.audition_id, auditionData.id, 10, (status, response) =>
        {
            if (status)
            {
                
            }
        });
    }
}
