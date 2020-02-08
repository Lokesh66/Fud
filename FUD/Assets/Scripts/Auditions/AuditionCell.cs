using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionCell : MonoBehaviour
{
    Audition auditionData;
    int index = 0;

    public void SetView(int index, Audition audition)
    {
        auditionData = audition;
        this.index = index;
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
    }
}
