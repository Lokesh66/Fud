using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Singleton

    private static DataManager instance = null;
    private DataManager()
    {

    }

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            return instance;
        }
    }

    #endregion
    public List<Craft> crafts = new List<Craft>();

    public List<Genre> genres = new List<Genre>();

    public UserData userInfo;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCrafts(List<Craft> data)
    {
        crafts = data;
    }
    public void UpdateGenres(List<Genre> data)
    {
        genres = data;
    }

    public void UpdateUserInfo(UserData userData)
    {
        userInfo = userData;
    }
}

#region UserData
[Serializable]
public class UserData
{
    public int id;
    public string name;
    public string token;
    public long phone;
    public int role_id;
    public object plan_id;
    public int age;
    public int login_code;
    public int token_expiry;
    public int agree_terms_condition;
    public string email_id;
    public string maa_membership_id;
    public object privacy_policy;
    public string current_location;
    public string native_location;
    public object reffered_by;
    public string device_token;
    public string dob;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class UserDataObject : BaseResponse
{
    public UserData data;
}
#endregion

#region UserLogin
[Serializable]
public class UserLoginData
{
    public int id;
    public string name;
    public string token;
    public long phone;
    public int role_id;
    public object plain_id;
    public int login_code;
    public int token_expiry;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class UserLoginbject
{
    public string message;
    public int status;
    public UserLoginData data;
}

#endregion

#region CRAFTS
[Serializable]

public class CraftsResponse
{
    public string message;
    public int status;
    public List<Craft> data = new List<Craft>();
}

[Serializable]
public class Craft
{
    public int id;
    public string name;
    public int status;
    public object image_url;
    public DateTime created_date_time;
    public DateTime updatedAt;
}
#endregion

#region GENRES
[Serializable]
public class GenreResponse 
{
    public List<Genre> data = new List<Genre>();
    public string message;
    public int status;
}
[Serializable]
public class Genre
{
    public int id;
    public string name;
    public DateTime created_date_time;
    public DateTime update_date_time;
    public int status;
}

#endregion


