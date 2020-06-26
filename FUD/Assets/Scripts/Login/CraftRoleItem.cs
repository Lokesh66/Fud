using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftRoleItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public Image icon;

    public Toggle toggle;

    public Image toggleImage;

    public Sprite defaultIcon;

    [Space]

    public Color selectedColor;

    [Space]


    Craft craft;
    System.Action<Craft> OnSelect = null;

    public void SetView(Craft craftInfo, System.Action<Craft> action, ToggleGroup toggleGroup)
    {
        craft = craftInfo;

        OnSelect = action;

        nameText.text = craftInfo.name;

        toggle.group = toggleGroup;

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

    public void OnClick(Toggle toggle)
    {
        if (toggle.isOn)
        {
            OnSelect?.Invoke(craft);
        }

        toggleImage.color = toggle.isOn ? selectedColor : Color.white;
    }
}
