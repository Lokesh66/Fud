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
        if (DataManager.Instance.CanLoadScreen(EFeatureType.AuditionCreation))
        {
            CreateAuditionView.Instance.SetView(1, (isNewDataUpdated) => {

                if (isNewDataUpdated)
                {

                }
            });
        }
        else
        {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.AuditionCreation);
        }
    }
}
