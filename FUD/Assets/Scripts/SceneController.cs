using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public enum ESceneType
    { 
        SplashScene,
        LoginScene,
        HomeScene
    }

    public ESceneType currentScene;

    public ESceneType GetCurrentScene()
    {
        return currentScene;
    }
}
