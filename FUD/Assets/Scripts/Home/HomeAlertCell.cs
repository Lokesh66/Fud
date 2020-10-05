using UnityEngine;
using TMPro;


public class HomeAlertCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI commentText;


    public void Load(HomeAlertModel alertModel, float width)
    {
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);

        titleText.text = alertModel.title;

        commentText.text = alertModel.comments;
    }
}
