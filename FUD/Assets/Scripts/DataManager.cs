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

    public void UpdateCrafts(List<Craft> data)
    {
        crafts = data;
    }
    public void UpdateGenres(List<Genre> data)
    {
        genres = data;
    }
}

#region UserData
[SerializeField]
public class UserData
{
    public int id;
    public string name;
    public long phone;
    public int role_id;
    public int login_code;
    public DateTime updatedAt;
    public DateTime created_date_time;
}

[SerializeField]
public class UserDataObject
{
    public string message;
    public int status;
    public UserData data;
}
#endregion

#region UserLogin
[SerializeField]
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

[SerializeField]
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


