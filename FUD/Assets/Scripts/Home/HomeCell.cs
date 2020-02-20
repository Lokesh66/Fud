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

    public void SetView(HomeCellType type, int index, string title, System.Action<int> onClick)
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
            case HomeCellType.AUDITION:
                addNewContent.SetActive(false);
                titleText.text = title;
                break;
        }
    }

    public void OnSelect()
    {
        OnClick?.Invoke(index);
        //OnClick = null;
    }
}
