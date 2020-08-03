using UnityEngine;
using TMPro;


public class HomeAlertCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI commentText;


    public void Load(HomeAlertModel alertModel)
    {
        titleText.text = alertModel.title;

        commentText.text = alertModel.comments;
    }
}
