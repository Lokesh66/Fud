using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProjectCastCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;
    public TMP_Text descriptionText;

    Action<ProjectCast> OnTapAction;

    ProjectCast castData;
    int index = 0;

    public void SetView(int index, ProjectCast cast, Action<ProjectCast> OnTapAction)
    {
        castData = cast;
        this.OnTapAction = OnTapAction;
        this.index = index;
        if (castData != null)
        {
            titleText.text = castData.id.ToString();
            ageText.text = "Project "+castData.project_id;
        }
    }

    public void OnClickAction()
    {
        OnTapAction?.Invoke(castData);
    }
}
