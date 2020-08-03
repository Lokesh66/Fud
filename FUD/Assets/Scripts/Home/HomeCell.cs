using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum HomeCellType
{
    DEFAULT,
    STORY,
    AUDITION
}
public class HomeCell : MonoBehaviour
{
    public HomeCellType cellType;

    public Image iconImage;
    public TMP_Text titleText;

    public GameObject addNewContent;

    private int index = -1;

    System.Action<int> OnClick;

    public void SetView(HomeCellType type, int index, string title, string url, System.Action<int> onClick)
    {
        this.index = index;
        OnClick = onClick;
        cellType = type;
        switch (type)
        {
            case HomeCellType.DEFAULT:
                addNewContent.SetActive(true);
                titleText.text = string.Empty;
                break;
            case HomeCellType.STORY:
                addNewContent.SetActive(false);
                titleText.text = title;
                /*GameManager.Instance.downLoadManager.DownloadImage(model.PortfolioMedia[0].content_url, (sprite) =>
                {
                    iconImage.sprite = sprite;
                });*/
                break;
            case HomeCellType.AUDITION:
                addNewContent.SetActive(false);
                UpdateImage(url);
                titleText.text = title;
                break;
        }
    }

    void UpdateImage(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            GameManager.Instance.downLoadManager.DownloadImage(url, (sprite) => {

                if (this != null)
                {
                    iconImage.sprite = sprite;
                }
            });
        }
    }

    public void OnSelect()
    {
        OnClick?.Invoke(index);
        //OnClick = null;
    }
}
