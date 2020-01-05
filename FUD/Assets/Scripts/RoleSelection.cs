using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelection : MonoBehaviour
{
    public CraftRoleItem roleItemPrefab;

    public Transform parentTransform;

    List<CraftRoleItem> itemList;

    System.Action<CraftRoleItem> OnItemSelected = null;
    // Start is called before the first frame update
    public void SetView(System.Action<CraftRoleItem> action)
    {
        gameObject.SetActive(true);

        OnItemSelected = action;
        itemList = new List<CraftRoleItem>();

        List<Craft> craftsList = DataManager.Instance.crafts;
        for(int i = 0; i < craftsList.Count; i++)
        {
            CraftRoleItem item = Instantiate<CraftRoleItem>(roleItemPrefab, parentTransform);
            item.SetView(craftsList[i],OnItemSelected);
            itemList.Add(item);
        }
    }

    void OnItemClick(CraftRoleItem item)
    {
        OnItemSelected?.Invoke(item);
        OnItemSelected = null;
    }

}
