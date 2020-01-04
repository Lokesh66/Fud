using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public GameObject launchScreen;
    private void Start()
    {
        GameManager.Instance.apiHandler.GetCraftRoles();
        GameManager.Instance.apiHandler.GetGenres();
        Invoke("GoToLaunchScreen",2.0f);
    }

    void GoToLaunchScreen()
    {
        this.gameObject.SetActive(false);
        launchScreen.gameObject.SetActive(true);
    }
}
