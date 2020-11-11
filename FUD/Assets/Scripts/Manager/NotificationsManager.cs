using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class NotificationsManager : MonoBehaviour
{

    #region Singleton

    private static NotificationsManager instance = null;

    private NotificationsManager()
    {

    }

    public static NotificationsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NotificationsManager>();
            }
            return instance;
        }
    }

    #endregion

    FirebaseApp app;

    string firebaseToken;


    public void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        //GalleryManager.Instance.loadingCountText.text += "token = " + SystemInfo.deviceUniqueIdentifier;

        firebaseToken = token.Token;

        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
    }

    public string GetFCMToken()
    {
        return firebaseToken;
    }
}
