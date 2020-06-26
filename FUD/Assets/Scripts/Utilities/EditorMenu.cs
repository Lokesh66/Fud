#if UNITY_EDITOR

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.IO;

#endif

#if UNITY_EDITOR
public class EditorMenu : MonoBehaviour
{
    [MenuItem("Tools/ClearData")]
    static void ClearData()
    {
        Directory.Delete(APIConstants.PERSISTENT_PATH, true);
    }

    [MenuItem("Tools/SwitchToHomeScene")]
    static void SwitchToHomeScene()
    {
        //UnityEditor.SceneManagement.EditorSceneManager.SetActiveScene();
    }
}

#endif
