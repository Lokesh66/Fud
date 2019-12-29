using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIConstants : MonoBehaviour
{
    #region Singleton
    private static APIConstants instance = null;
    private APIConstants()
    {

    }

    public static APIConstants Instance
    {
        get
        {
            if (instance = null)
            {
                instance = FindObjectOfType<APIConstants>();
            }
            return instance;
        }
    }
    #endregion
    public const string SECRET_KEY = "SSht6KyxvwKZnsmbbbQUCm2va";
    public const string BASE_URL = "http://3.136.106.58:7000/";

    public const string USER = BASE_URL + "v1/users/";
    public const string USER_LOGIN = BASE_URL + "v1/login/";
    public const string STORY = BASE_URL + "v1/story/";

    public const string GET_CRATFS = BASE_URL + "v1/24_craftRoles/";
    public const string GET_GENRES = BASE_URL + "v1/genres/";


    public class BaseData
    {
        public string message;
        public int status;
    }

    #region UserData
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

    public class UserDataObject : BaseData
    {
        public UserData data;
    }
    #endregion

    #region UserLogin
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

    public class UserLoginbject : BaseData
    {
        public UserLoginData data;
    }

    #endregion

    #region CRAFTS
    public class Craft
    {
        public int id;
        public string name;
        public int status;
        public object image_url;
        public DateTime created_date_time;
        public DateTime updatedAt;
    }

    public class CraftsResponse : BaseData
    {
        public List<Craft> data;
    }
    #endregion

    #region GENRES
    public class Genre
    {
        public int id;
        public string name;
        public DateTime created_date_time;
        public DateTime? update_date_time;
        public int? status;
    }

    public class RootObject : BaseData
    {
        public List<Genre> message;
    }
    #endregion

}
