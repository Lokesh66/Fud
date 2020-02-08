using UnityEngine;

public class AuditionsPanel : MonoBehaviour
{
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
        CreateAuditionView.Instance.SetView(() => { 
        
        });
    }
}
