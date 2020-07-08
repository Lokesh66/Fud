#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;

#endif

#if UNITY_EDITOR
public class EditorMenu : MonoBehaviour
{
    [MenuItem("Tools/ClearData")]
    static void ClearData()
    {
        Directory.Delete(APIConstants.PERSISTENT_PATH, true);
    }

    [MenuItem("Scenes/SwitchToHomeScene")]
    static void SwitchToHomeScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/HomeScene.unity");
    }

    [MenuItem("Scenes/SwitchToDefaultScene")]
    static void SwitchToDefaultScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/SplashScreen.unity");

        EditorApplication.isPlaying = true;
    }
}

#endif
