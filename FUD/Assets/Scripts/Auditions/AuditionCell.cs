using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionCell : MonoBehaviour
{
    int index = 0;

    public void SetView(int index)
    {
        this.index = index;
    }

    public void OnButtonAction()
    {
        if (index == 0)
        {
            //Create Audition
        }
        else { 
            //Call API for AuditionType Selection list
        }
    }
}
