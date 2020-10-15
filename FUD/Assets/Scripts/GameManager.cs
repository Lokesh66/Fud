﻿using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System;
using UMP;
using UnityEngine.Events;

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

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern float initializeWithAppID(string appId, EPurchaseType purchaseType);

#endif

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
        Debug.Log("Temp Path = " + Application.persistentDataPath);
        //File.Delete(APIConstants.TOKEN_PATH);

        StartGame();
    }

    void Init()
    {
        apiHandler = new APIHandler(this);

        sceneController = new SceneController();
    }

    void StartGame()
    {
        apiHandler.GetCraftRoles();

        apiHandler.GetCastRoles(0);

        apiHandler.GetCrewRoles(1);

        apiHandler.GetGenres();

        apiHandler.GetMediaEndPoint();

#if UNITY_IOS && !UNITY_EDITOR
        initializeWithAppID(APIHandler.APP_ID, EPurchaseType.Test);
#endif

        if (File.Exists(APIConstants.TOKEN_PATH))
        {
            sceneController.SwitchScene(ESceneType.HomeScene);
        }
        else {
            sceneController.SwitchScene(ESceneType.LoginScene);
        }
    }

    public void SessionExpired()
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = "Session expired. \n Please login again.";

        alertModel.okayButtonAction = SwitchToLogin;

        UIManager.Instance.ShowAlert(alertModel);
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
