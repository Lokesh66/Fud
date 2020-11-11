using UnityEngine.SceneManagement;

public enum ESceneType
{
    SplashScene,
    LoginScene,
    HomeScene
}

public class SceneController
{
    public ESceneType currentScene;

    public ESceneType GetCurrentScene()
    {
        return currentScene;
    }

    public void SwitchScene(ESceneType eSceneType)
    {
        SceneManager.LoadScene(eSceneType.ToString());
    }
}
