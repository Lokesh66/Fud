using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftRoleItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public Image icon;

    public Image toggleImage;

    public Sprite defaultIcon;

    [Space]

    public Color selectedColor;

    [Space]


    Craft craft;
    System.Action<Craft> OnSelect = null;

    public void SetView(Craft craftInfo, System.Action<Craft> action)
    {
        craft = craftInfo;

        OnSelect = action;

        nameText.text = craftInfo.name;

        SetImage();
    }

    void SetImage()
    {
        icon.sprite = Resources.Load<Sprite>("Images/RoleSelection/" + craft.name);

        if (icon.sprite == null)
        {
            icon.sprite = defaultIcon;
        }
    }

    public void OnClick()
    {
        OnSelect?.Invoke(craft);
    }
}
