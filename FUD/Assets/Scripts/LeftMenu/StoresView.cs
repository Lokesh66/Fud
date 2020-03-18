using UnityEngine;
using TMPro;


public class StoresView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storeCell;

    public void EnableView()
    {
        gameObject.SetActive(true);

        Load();
    }

    void Load()
    { 
        
    }

    public void ClearData()
    { 
        
    }
}
