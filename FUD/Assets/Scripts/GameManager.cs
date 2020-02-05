using UnityEngine;
using System.IO;

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

}
