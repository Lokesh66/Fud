﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public TopCanvas topCanvas;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Init();
    }

    void Init()
    {
        
    }

    #region Un Available Features
    public void CreateUnAvaiableAlert(EFeatureType featureType)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = GetUnAvailableMessage(featureType);

        topCanvas.alertView.ShowAlert(alertModel);
    }

    string GetUnAvailableMessage(EFeatureType featureType)
    {
        string message = string.Empty;

        switch (featureType)
        {
            case EFeatureType.StoryCreation:
                message = "Don't have enough stories for creation";
                break;

            case EFeatureType.StoryVersionCreation:
                message = "Don't have enough story versions for creation";
                break;

            case EFeatureType.ShareStoryVersion:
                message = "You can't share story version";
                break;

            case EFeatureType.PortfolioCreation:
                message = "You cann't create Portfolio";
                break;

            case EFeatureType.PortfolioAlbums:
                message = "You don't have enough available albums to upload";
                break;

            case EFeatureType.ProjectCreation:
                message = "You cann't create Project";
                break;

            case EFeatureType.AuditionCreation:
                message = "You cann't create Audition";
                break;

            case EFeatureType.AuditionJoining:
                message = "You cann't join Audition";
                break;

        }
        return message;
    }

    #endregion

    #region Alerts

    public void ShowAlert(AlertModel alertModel)
    {
        Debug.LogError("UIManager : Show Alert Called");

        topCanvas.alertView.ShowAlert(alertModel);
    }

    #endregion
}
