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

    public RectTransform canvasTrans;

    public AlertViewController alertView;

    public LeftMenu leftMenu;
}
