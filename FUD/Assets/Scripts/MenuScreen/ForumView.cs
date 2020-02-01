using UnityEngine;

public class ForumView : BaseView
{
    protected override void EnableView()
    {
        //Enable View will be called on clicking the bottom button
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
}
