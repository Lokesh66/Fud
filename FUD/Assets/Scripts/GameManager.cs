using UnityEngine;
using System.IO;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance = null;

    private GameManager()
    {

    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    #endregion

    // Start is called before the first frame update

    [HideInInspector]
    public APIHandler apiHandler;

    [HideInInspector]
    public SceneController sceneController;

    public DownLoadManager downLoadManager;    

    public RectTransform loaderTrans;

    Tweener loaderTween;

    void Awake()
    {
        Init();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    void Init()
    {
        apiHandler = new APIHandler(this);

        sceneController = new SceneController();
    }

    void StartGame()
    {
        if (File.Exists(APIConstants.TOKEN_PATH))
        {
            sceneController.SwitchScene(ESceneType.HomeScene);
        }
        else {
            sceneController.SwitchScene(ESceneType.LoginScene);
        }
    }

    public void StartLoading()
    {
        loaderTrans.gameObject.SetActive(true);

        loaderTween = loaderTrans.DOLocalRotate(new Vector3(0, 0, 360), 0.8f).SetLoops(-1);
    }

    public void StopLoading()
    {
        loaderTween?.Kill();

        loaderTrans.gameObject.SetActive(false);
    }
}
