using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    #region Singleton

    private static Loader instance = null;

    private Loader()
    {

    }

    public static Loader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Loader>();
            }
            return instance;
        }
    }
    #endregion

    public GameObject loaderPanel;
    public Image loaderImage;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartLoading()
    {
        if (!loaderPanel.activeSelf)
        {
            loaderPanel.SetActive(true);

            loaderImage.rectTransform.DOLocalRotate(new Vector3(0, 0, 360), 0.8f).SetLoops(-1);
        }
    }

    public void StopLoading()
    {
        DOTween.Kill(loaderImage.rectTransform);

        loaderPanel.SetActive(false);
    }
}
