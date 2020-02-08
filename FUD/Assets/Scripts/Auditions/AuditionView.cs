using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionView : BaseView
{
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

    public void OnAddButtonAction()
    {
        CreateAuditionView.Instance.SetView(() => { 
        
        });
    }
}
