using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftRoleItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image icon;

    System.Action<CraftRoleItem> OnSelect = null;

    public void SetView(Craft craftInfo, System.Action<CraftRoleItem> action)
    {
        OnSelect = action;
        /*GameManager.Instance.apiHandler.DownloadImage(craftInfo.image_url.ToString(), (Sprite img) =>{
            if(img != null)
            {
                icon.sprite = img;
            }
        });*/
        nameText.text = craftInfo.name;
    }
    
    public void OnClick()
    {
        OnSelect?.Invoke(this);
        OnSelect = null;
    }
}
