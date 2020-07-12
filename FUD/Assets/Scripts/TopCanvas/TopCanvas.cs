using UnityEngine.UI;
using UnityEngine;
using System;


public class TopCanvas : MonoBehaviour
{
    public RectTransform rectTransform;

    public AlertViewController alertView;

    public BigScreenView bigScreenView;


    private Action<object> OnClose;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void ShowScreen(GameObject _screenObject, Action<object> OnClose = null)
    {
        this.OnClose = OnClose;

        GameObject screenObject = Instantiate(_screenObject, rectTransform);
    }

    public void ShowBigScreen(string imageURL)
    {
        bigScreenView.Load(imageURL);
    }
}
