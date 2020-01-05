using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.apiHandler.GetCraftRoles();
        GameManager.Instance.apiHandler.GetGenres();
        Invoke("GoToLaunchScreen", 2.0f);
    }

    void GoToLaunchScreen()
    {
        GameManager.Instance.sceneController.SwitchScene(ESceneType.LoginScene);
    }
}
