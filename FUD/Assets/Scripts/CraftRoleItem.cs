using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftRoleItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image icon;

    Craft craft;
    System.Action<Craft> OnSelect = null;

    public void SetView(Craft craftInfo, System.Action<Craft> action)
    {
        craft = craftInfo;
        OnSelect = action;

        nameText.text = craftInfo.name;
        /*GameManager.Instance.apiHandler.DownloadImage(craftInfo.image_url.ToString(), (Sprite img) =>{
            if(img != null)
            {
                icon.sprite = img;
            }
        });*/
    }
    
    public void OnClick()
    {
        OnSelect?.Invoke(craft);
        OnSelect = null;
    }
}
