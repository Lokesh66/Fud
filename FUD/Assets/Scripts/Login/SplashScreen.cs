using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        Invoke("GoToLaunchScreen", 2.0f);
    }

    void GoToLaunchScreen()
    {
        GameManager.Instance.sceneController.SwitchScene(ESceneType.LoginScene);
    }
}
