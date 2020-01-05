using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelectionScreen : MonoBehaviour
{
    public CraftRoleItem roleItemPrefab;

    public Transform parentTransform;

    List<CraftRoleItem> itemList;

    System.Action<Craft> OnItemSelected = null;


    public void SetView(System.Action<Craft> action)
    {
        gameObject.SetActive(true);

        OnItemSelected = action;
        itemList = new List<CraftRoleItem>();

        List<Craft> craftsList = DataManager.Instance.crafts;
        for(int i = 0; i < craftsList.Count; i++)
        {
            CraftRoleItem item = Instantiate<CraftRoleItem>(roleItemPrefab, parentTransform);
            item.SetView(craftsList[i], OnItemSelected);
            itemList.Add(item);
        }
    }

    void OnItemClick(Craft item)
    {
        OnItemSelected?.Invoke(item);
        OnItemSelected = null;
    }

    public void OnClick_Back()
    {
        OnItemSelected?.Invoke(null);
        OnItemSelected = null;
    }

}
