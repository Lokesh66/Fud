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

    public Texture2D defaultTexture;

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
                        icon.texture = defaultTexture;
                    }
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
                     Rect rect = new Rect(0, 0, icon.rectTransform.rect.width, icon.rectTransform.rect.height);

                     Sprite sprite = Sprite.Create(texture.ToTexture2D(), rect, new Vector2(0.5f, 0.5f));

                     //icon.sprite = sprite;

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
