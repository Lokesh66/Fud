using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneType
{
    SplashScene,
    LoginScene,
    HomeScene
}

public class SceneController : MonoBehaviour
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
