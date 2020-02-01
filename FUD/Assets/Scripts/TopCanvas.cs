using UnityEngine;
using System;

public class TopCanvas : MonoBehaviour
{
    private static TopCanvas instance;

    public static TopCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TopCanvas>();
            }

            return instance;
        }
    }

    public RectTransform rectTransform;

    private Action<object> OnClose;

    public void ShowScreen(GameObject _screenObject, Action<object> OnClose = null)
    {
        this.OnClose = OnClose;

        GameObject screenObject = Instantiate(_screenObject, rectTransform);
    }
}
