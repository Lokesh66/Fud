using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

public partial class APIHandler
{
    public void SendOTP(string phoneNumber, Action<bool> action)
    {
        Debug.Log("SendOTP");
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        parameters.Add("phone", phoneNumber);

        PostRequest(APIConstants.USER_OTP, parameters, (bool status, string response) => {
            Debug.LogFormat("<LoginAPIHandler/SendOTP> Response ({0})", response);
            action?.Invoke(status);

        });
    }
}
