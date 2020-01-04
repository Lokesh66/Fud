using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScreen : MonoBehaviour
{
    public GameObject signUpScreen;
    public GameObject loginScreen;
    public void OnClickCreateNewAccount()
    {
        this.gameObject.SetActive(false);
        signUpScreen.gameObject.SetActive(true);
    }

    public void OnClickLogin()
    {
        this.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(true);
    }

}
