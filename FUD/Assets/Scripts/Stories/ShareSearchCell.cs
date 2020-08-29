using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using UnityEngine;
using System;
using TMPro;


public class ShareSearchCell : MonoBehaviour
{
    public TextMeshProUGUI userNameText;

    public RemoteImageBehaviour remoteImageBehaviour;


    Action<object> OnButtonPressed;

    UserSearchModel searchModel;

    public void SetView(UserSearchModel searchModel, Action<object> onButtonPressed)
    {
        this.OnButtonPressed = onButtonPressed;

        this.searchModel = searchModel;

        userNameText.text = searchModel.name;

        remoteImageBehaviour.Load(searchModel.profile_image);
    }

    public void OnButtonAction()
    {
        OnButtonPressed?.Invoke(searchModel);
    }
}
