using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

public partial class APIHandler : MonoBehaviour
{
    public void SendOTP(string phoneNumber, Action<bool> action)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        parameters.Add("phone", phoneNumber);

        PostRequest(APIConstants.USER_OTP, parameters, (bool status, string response) => {

            action?.Invoke(status);

        });
    }
}
