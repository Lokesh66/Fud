using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public RectTransform leftWheelTrans;

    public RectTransform rightWheelTrans;


    Vector3 rotationValue = new Vector3(0, 0, 360);

    RotateMode rotateMode = RotateMode.FastBeyond360;

    Sequence mySequence = null;



    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartLoading()
    {
        if (!loaderPanel.activeSelf)
        {
            loaderPanel.SetActive(true);

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(leftWheelTrans.DOLocalRotate(rotationValue, 0.6f, rotateMode).SetEase(Ease.Linear));

            mySequence.Join(rightWheelTrans.DOLocalRotate(rotationValue, 0.6f, rotateMode).SetEase(Ease.Linear));

            this.mySequence = mySequence.Play().SetLoops(-1);
        }
    }

    public void StopLoading()
    {
        mySequence.onKill += OnKill;

        mySequence?.Kill(true);

        loaderPanel.SetActive(false);
    }

    void OnKill()
    {
        mySequence.onKill -= OnKill;

        leftWheelTrans.eulerAngles = Vector3.zero;

        rightWheelTrans.eulerAngles = Vector3.zero;
    }
}
