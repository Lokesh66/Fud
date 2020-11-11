using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class SceneAlbumCell : MonoBehaviour
{
    public GameObject selectInActiveObject;

    public GameObject selectActiveObject;

    public GameObject selectToggle;


    public TextMeshProUGUI userNameText;


    Action<object> OnButtonAtion;

    Action<SceneAlbumCell, object> OnSelectAction;

    SceneAlbumModel searchModel;

    SceneCharacter sceneCharacter;

    bool isView = false;



    public void SetView(SceneAlbumModel searchModel, Action<object> OnButtonAtion, Action<SceneAlbumCell, object> OnSelectAction, bool canAddCharacters = false)
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

        selectInActiveObject.SetActive(canAddCharacters);

        selectToggle.SetActive(canAddCharacters);
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
        if (isView)
        {
            OnButtonAtion?.Invoke(sceneCharacter);
        }
        else
        {
            OnButtonAtion?.Invoke(searchModel);
        }
    }

    #region Scene View

    public void SetView(SceneCharacter sceneCharacter, Action<object> OnButtonAtion)
    {
        this.sceneCharacter = sceneCharacter;

        this.OnButtonAtion = OnButtonAtion;

        selectToggle.SetActive(false);

        isView = true;

        userNameText.text = sceneCharacter.Users.name;
    }

    #endregion

    #region Update View

    public void SetView(SceneCharacter sceneCharacter, Action<object> OnButtonAtion, Action<SceneAlbumCell, object> OnSelectAction, bool canAddCharacters = false)
    {
        this.sceneCharacter = sceneCharacter;

        this.OnButtonAtion = OnButtonAtion;

        this.OnSelectAction = OnSelectAction;

        selectInActiveObject.SetActive(OnSelectAction != null);

        userNameText.text = sceneCharacter.Users.name;

        if (OnSelectAction != null)
        {
            selectActiveObject.SetActive(searchModel.isSeeAllSelected);
        }

        selectInActiveObject.SetActive(canAddCharacters);

        selectToggle.SetActive(canAddCharacters);
    }

    #endregion
}
