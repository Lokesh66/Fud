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

                Debug.Log("model = " + model);

                if (model != null)
                {
                    auditionData.onScreenModel = model;
                }
                else {
                    EMediaType mediaType = modelsList[0].GetMediaType(modelsList[0].media_type);

                    Debug.Log("mediaType = " + mediaType);

                    if (mediaType == EMediaType.Video)
                    {
                        mediaPlayer.Path = modelsList[0].content_url;

                        mediaPlayer.Prepare();

                        mediaPlayer.AddEndReachedEvent(() =>
                        {
                            VideoStreamer.Instance.updatedRawImage.gameObject.SetActive(false);
                        });

                        mediaPlayer.AddImageReadyEvent((texture) =>
                        {
                            icon.texture = texture;
                        });
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

    public void OnVideoPlayButtonAction()
    {
        UIManager.Instance.topCanvas.PlayVideo(icon, mediaPlayer);
    }
}
