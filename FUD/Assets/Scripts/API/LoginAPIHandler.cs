using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;
using System.IO;

public partial class APIHandler
{
    public void SendOTP(long phoneNumber, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("phone", phoneNumber);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_OTP, false, parameters, (bool status, string response) => {
            Debug.LogFormat("<LoginAPIHandler/SendOTP> Response ({0})", response);
            action?.Invoke(status, response);

        }));
    }

    public void SignIn(Dictionary<string, object> parameters, Action<bool, UserData> action)
    {
        //parameters.Add("login_code", "1234");// code);
        Debug.Log("device_token : "+SystemInfo.deviceUniqueIdentifier);

        string deviceToken = NotificationsManager.Instance.GetFCMToken();

        deviceToken = deviceToken.IsNOTNullOrEmpty() ? deviceToken : string.Empty;

        parameters.Add("device_token", deviceToken);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_USER, false, parameters, (bool status, string response) => {

            if (status)
            {
                UserDataObject loginResponse = JsonUtility.FromJson<UserDataObject>(response);

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
                BaseResponse loginResponse = JsonUtility.FromJson<BaseResponse>(response);

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

        string deviceToken = NotificationsManager.Instance.GetFCMToken();

        deviceToken = deviceToken.IsNOTNullOrEmpty() ? deviceToken : string.Empty;

        parameters.Add("phone", phoneNumber);
        parameters.Add("login_code", code);

        parameters.Add("device_token", deviceToken);

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

    public void GetCountries(Action<bool, List<CountryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        gameManager.StartCoroutine(GetRequest(APIConstants.GET_COUNTRIES, false, (bool status, string response) => {

            Debug.Log("Countries Respopnse = " + response);

            CountriesResponse responseModel = JsonUtility.FromJson<CountriesResponse>(response);

            if (status)
            {
                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }
        }));
    }
}

[Serializable]
public class CountriesResponse : BaseResponse
{
    public List<CountryModel> data;
}

[Serializable]
public class CountryModel
{
    public int id;
    public string name;
    public string iso;
    public int numcode;
    public int phonecode;
    public string nicename;
    public string iso3;
}

