using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortfolioBasicInfo : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI mailText;

    public TextMeshProUGUI roleText;

    public TextMeshProUGUI addressText;


    public void Load()
    {
        SetView();
    }

    void SetView()
    {
        UserData data = DataManager.Instance.userInfo;

        if (data != null)
        {
            Genre selectedGenre = DataManager.Instance.genres.Find(genre => genre.id == data.role_id);

            nameText.text = data.name;

            mailText.text = data.email_id;

            roleText.text = selectedGenre.name;

            addressText.text = data.native_location;
        }
    }
}
