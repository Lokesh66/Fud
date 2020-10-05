using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    #region Singleton

    private static CanvasManager instance = null;

    private CanvasManager()
    {

    }

    public static CanvasManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CanvasManager>();
            }
            return instance;
        }
    }
    #endregion

    public LeftMenu leftMenu;


    public float GetCanvasWidth()
    {
        return (transform as RectTransform).rect.width;
    }
}
