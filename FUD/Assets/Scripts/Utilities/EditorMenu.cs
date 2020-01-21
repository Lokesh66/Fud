#if UNITY_EDITOR

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
}

#endif
