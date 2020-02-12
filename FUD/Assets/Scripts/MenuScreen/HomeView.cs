using UnityEngine;

public class HomeView : BaseView
{
    public GameObject leftMenuPanel;


    #region Ovveride Methods
    protected override void EnableView()
    {
        base.EnableView();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();
    }

    #endregion

    public void OnMenuButtonAction()
    {
        leftMenuPanel.SetActive(true);
    }

    public void OnCloseLeftMenu()
    {
        leftMenuPanel.SetActive(false);
    }
}
