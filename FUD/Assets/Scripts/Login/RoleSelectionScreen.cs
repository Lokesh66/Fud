using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class RoleSelectionScreen : MonoBehaviour
{
    public CraftRoleItem roleItemPrefab;

    public Transform parentTransform;

    public ToggleGroup toggleGroup;


    List<CraftRoleItem> itemList;

    Craft selectedItem;

    System.Action<Craft> OnConfirmRole = null;


    public void SetView(System.Action<Craft> action)
    {
        gameObject.SetActive(true);

        parentTransform.DestroyChildrens();

        OnConfirmRole = action;

        itemList = new List<CraftRoleItem>();

        List<Craft> craftsList = DataManager.Instance.crafts;

        for(int i = 0; i < craftsList.Count; i++)
        {
            CraftRoleItem item = Instantiate(roleItemPrefab, parentTransform);

            //item.SetView(craftsList[i], OnItemSelected, toggleGroup);

            itemList.Add(item);
        }
    }

    void OnItemSelected(Craft selectedItem)
    {
        this.selectedItem = selectedItem;
    }

    public void OnItemClick()
    {
        OnConfirmRole?.Invoke(selectedItem);

        OnConfirmRole = null;

        selectedItem = null;
    }

    public void OnClick_Back()
    {
        OnConfirmRole?.Invoke(null);

        OnConfirmRole = null;
    }
}
