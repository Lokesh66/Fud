using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public GameObject launchScreen;
    private void Start()
    {
        APIManager.Instance.GetCraftRoles();
        APIManager.Instance.GetGenres();
        Invoke("GoToLaunchScreen",2.0f);
    }

    void GoToLaunchScreen()
    {
        this.gameObject.SetActive(false);
        launchScreen.gameObject.SetActive(true);
    }
}
