using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class SceneAlbumCell : MonoBehaviour
{
    public GameObject selectInActiveObject;

    public GameObject selectActiveObject;


    public TextMeshProUGUI userNameText;


    Action<object> OnButtonAtion;

    Action<SceneAlbumCell, object> OnSelectAction;

    SceneAlbumModel searchModel; 



    public void SetView(SceneAlbumModel searchModel, Action<object> OnButtonAtion, Action<SceneAlbumCell, object> OnSelectAction)
    {
        this.OnButtonAtion = OnButtonAtion;

        this.searchModel = searchModel;

        this.OnSelectAction = OnSelectAction;

        selectInActiveObject.SetActive(OnSelectAction != null);

        userNameText.text = searchModel.Users.name;

        if (OnSelectAction != null)
        {
            selectActiveObject.SetActive(searchModel.isSeeAllSelected);
        }
    }

    public void UpdateDeselectView()
    {
        selectActiveObject.SetActive(false);
    }

    public void OnSelectButtonAction()
    {
        selectActiveObject.SetActive(!selectActiveObject.activeSelf);

        OnSelectAction?.Invoke(this, searchModel);
    }

    public void OnCellButtonAction()
    {
        OnButtonAtion?.Invoke(searchModel);
    }
}
