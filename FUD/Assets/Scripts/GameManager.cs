using UnityEngine;
using System.IO;
using System;

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

    public void SwitchToLogin()
    {
        sceneController.SwitchScene(ESceneType.LoginScene);
    }

    public double GetCurrentEpochTime()
    {
        double epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        return epoch;
    }

    public string epoch2string(int epoch)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch).ToShortDateString();
    }
}
