using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionsPanel : MonoBehaviour
{

    public CreateAuditionView createAuditionView;

    private void OnEnable()
    {
        GetAuditions();
    }
    void GetAuditions()
    {
        GameManager.Instance.apiHandler.GetAllAuditions((status, auditionsList) => {
            Debug.Log("GetAuditions : "+status);
        });
    }
    public void CreateAudition()
    {
        createAuditionView.SetView(() => { 
        
        });
    }
}
