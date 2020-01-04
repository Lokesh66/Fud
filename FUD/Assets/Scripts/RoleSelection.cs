using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelection : MonoBehaviour
{
    public GameObject roleItemPrefab;

    public Transform parentTransform;

    List<CraftRoleItem> itemList;

    System.Action<CraftRoleItem> OnItemSelected = null;
    // Start is called before the first frame update
    public void SetView(System.Action<CraftRoleItem> action)
    {
        this.gameObject.SetActive(true);

        OnItemSelected = action;
        List<Craft> craftsList = DataManager.Instance.crafts;
        for(int i = 0; i < craftsList.Count; i++)
        {
            GameObject item = Instantiate(roleItemPrefab, parentTransform) as GameObject;
            CraftRoleItem craftRoleItem = item.GetComponent<CraftRoleItem>();
            craftRoleItem.SetView(craftsList[i],OnItemSelected);
            itemList.Add(craftRoleItem);
        }
    }

    void OnItemClick(CraftRoleItem item)
    {
        this.gameObject.SetActive(false);
        OnItemSelected?.Invoke(item);
        OnItemSelected = null;
    }

}
