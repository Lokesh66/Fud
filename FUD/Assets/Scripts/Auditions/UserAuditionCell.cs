using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UserAuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;

    UserAudition auditionData;

    Action<UserAudition> OnSelect;

    public void SetView(UserAudition audition, Action<UserAudition> action)
    {
        auditionData = audition;
        OnSelect = action;

        if (auditionData != null)
        {
            titleText.text = auditionData.audition_id.ToString();
            ageText.text = auditionData.user_id.ToString();

            if (auditionData.UserAuditionMultimedia != null && auditionData.UserAuditionMultimedia.Count > 0)
            {
                GameManager.Instance.downLoadManager.DownloadImage(auditionData.UserAuditionMultimedia[0], (sprite) =>
                {
                    icon.sprite = sprite;
                });
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
