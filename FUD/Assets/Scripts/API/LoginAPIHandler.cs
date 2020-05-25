using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;
using System.IO;

public partial class APIHandler
{
    public void SendOTP(long phoneNumber, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("phone", phoneNumber);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_OTP, false, parameters, (bool status, string response) => {
            Debug.LogFormat("<LoginAPIHandler/SendOTP> Response ({0})", response);
            action?.Invoke(status);

        }));
    }

    public void SignIn(string name, long phoneNumber, int roleId, Action<bool, UserData> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("name", name);
        parameters.Add("phone", phoneNumber);
        parameters.Add("role_id", roleId);
        //parameters.Add("login_code", "1234");// code);
        Debug.Log("device_token : "+SystemInfo.deviceUniqueIdentifier);
        parameters.Add("device_token", SystemInfo.deviceUniqueIdentifier);
        parameters.Add("agree_terms_condition", 1);
        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_USER, false, parameters, (bool status, string response) => {

            UserDataObject loginResponse = JsonUtility.FromJson<UserDataObject>(response);

            if (status)
            {
                string userFilePath = APIConstants.PERSISTENT_PATH + "UserInfo";

                string fileName = Path.GetDirectoryName(userFilePath);

                if (!Directory.Exists(userFilePath))
                {
                    Directory.CreateDirectory(userFilePath);
                }

                userFilePath += "/Userinfo";

                File.WriteAllText(userFilePath, response);

                DataManager.Instance.UpdateUserInfo(loginResponse.data);

                gameManager.apiHandler.SetToken(loginResponse.data.token);
                action?.Invoke(true, loginResponse.data);
            }
            else
            {
                AlertModel alertModel = new AlertModel();

                alertModel.message = loginResponse.message;

                UIManager.Instance.ShowAlert(alertModel);

                action?.Invoke(false, null);
            }

        }));
    }

    public void Login(long phoneNumber, long code, Action<bool, UserData> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("phone", phoneNumber);
        parameters.Add("login_code", code);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_LOGIN, false, parameters, (bool status, string response) => {
            if (status)
            {
                string userFilePath = APIConstants.PERSISTENT_PATH + "UserInfo";

                string fileName = Path.GetDirectoryName(userFilePath);

                if (!Directory.Exists(userFilePath))
                {
                    Directory.CreateDirectory(userFilePath);
                }

                userFilePath += "/Userinfo";

                File.WriteAllText(userFilePath, response);

                UserDataObject loginResponse = JsonUtility.FromJson<UserDataObject>(response);

                DataManager.Instance.UpdateUserInfo(loginResponse.data);

                gameManager.apiHandler.SetToken(loginResponse.data.token);
                action?.Invoke(true, loginResponse.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void Logout(Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        gameManager.StartCoroutine(PutRequest(APIConstants.ACCOUNT_LOGOUT, true, parameters, (bool status, string response) => {

            if (status)
            {
                action?.Invoke(true, string.Empty);
            }
            else
            {
                action?.Invoke(false, string.Empty);
            }
        }));
    }
}

