using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

public partial class APIHandler
{
    public void SendOTP(long phoneNumber, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("phone", phoneNumber);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_OTP, parameters, (bool status, string response) => {
            Debug.LogFormat("<LoginAPIHandler/SendOTP> Response ({0})", response);
            action?.Invoke(status);

        }));
    }

    public void Login(long phoneNumber, string code, Action<bool, User> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("phone", phoneNumber);
        parameters.Add("login_code", "1234");// code);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_LOGIN, parameters, (bool status, string response) => {
            if (status)
            {
                LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);
                action?.Invoke(true, loginResponse.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }
}

[Serializable]
public class User
{
    public int id;
    public string name;
    public string token;
    public long phone;
    public int role_id;
    public object plain_id;
    public int login_code;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class LoginResponse
{
    public string message;
    public User data;
    public int status;
}
