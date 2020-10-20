using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
