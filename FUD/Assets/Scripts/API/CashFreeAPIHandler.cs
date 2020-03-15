using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public static bool IS_PRODUCTION = false;

    public const string APP_ID_KEY = "appId";
    public const string ORDER_ID_KEY = "orderId";
    public const string ORDER_AMOUNT_KEY = "orderAmount";
    public const string CUSTOMER_NAME_KEY = "customerName";
    public const string CUSTOMER_PHONE_KEY = "customerPhone";
    public const string CUSTOMER_EMAIL_KEY = "customerEmail";
    public const string ORDER_NOTE_KEY = "orderNote";

    public const string APP_ID = "1362262b24f262b46ab91599e22631";

    public void Example()
    {
        Dictionary<string, object> planIdInfo = new Dictionary<string, object>
        {
            { "plan_id", 1 }
        };
        CashFreeRequest ("911234567890", "GMAIL_ACCOUNT", planIdInfo);
    }

    public void CashFreeRequest (string customerPhone, string customerEmail, Dictionary<string, object>  planIdInfo, string customerName = null, string orderNote = null)
    {
        gameManager.apiHandler.PostRequest (APIConstants.GET_CASH_FREE_TOKEN, true, planIdInfo, (bool status, string responseString) => 
        {
            if (status)
            {
                CFPTokenResponse response = JsonUtility.FromJson<CFPTokenResponse> (responseString);
                Dictionary<string, string> parameters = response.ConvertToParameters ();

#if UNITY_ANDROID

                response.Add (APP_ID, APP_ID_KEY, APP_ID, parameters);
                response.Add (customerPhone, CUSTOMER_PHONE_KEY, customerPhone, parameters);
                response.Add (customerEmail, CUSTOMER_EMAIL_KEY, customerEmail, parameters);

                response.Add (orderNote, ORDER_NOTE_KEY, orderNote, parameters);            // Optional
                response.Add (customerName, CUSTOMER_NAME_KEY, customerName, parameters);   // Optional

                AndroidJavaObject paramObj = CreateJavaMapFromDictainary (parameters);


                if (string.IsNullOrEmpty (response.token))
                { 
                    response.token = GetCFPToken ();
                }

                if (string.IsNullOrEmpty(response.stage))
                {
                    response.stage = "TEST";
                }
                AndroidJavaObject currentActivity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject> ("currentActivity");

                AndroidJavaClass paymentActivity = new AndroidJavaClass ("com.phonatoto.cashfree.PaymentActivity");
                paymentActivity.CallStatic ("doPayment", currentActivity, paramObj, response.token, response.stage);

#else
        //TODO: iOS is pending
        Debug.Log("iOS Cash free implementation is pending");
#endif
            }
        });
    }

    string GetCFPToken()
    {
        string token = string.Empty;

        token = "fm9JCN4MzUIJiOicGbhJCLiQ1VKJiOiAXe0Jye.TN0nIjZWN4ITOmNWZkZTZ1IiOiQHbhN3XiwyM5MDN1gjN4UTM6ICc4VmIsIiUOlkI6ISej5WZyJXdDJXZkJ3biwCMwATM6ICduV3btFkclRmcvJCLxojIklkclRmcvJye.69a6v05mx9RTFh6ukHqeB3rQ8wqm6jvIBORicVcNUWIxi976sWQiO6NU09XwRD6tz8";

        return token;
    }

    AndroidJavaObject CreateJavaMapFromDictainary (Dictionary<string, string> parameters)
    {
#if UNITY_ANDROID
        AndroidJavaObject map = new AndroidJavaObject ("java.util.HashMap");
        IntPtr putMethod = AndroidJNIHelper.GetMethodID (map.GetRawClass (), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
        foreach (var entry in parameters)
        {
            AndroidJNI.CallObjectMethod (
                map.GetRawObject (),
                putMethod,
                AndroidJNIHelper.CreateJNIArgArray (new object [] { entry.Key, entry.Value })
            );
        }
        return map;
#endif
    }
}

[Serializable]
public class CFPTokenResponse
{
    public string appId;
    public string orderId;
    public string orderAmount;
    public string customerEmail;
    public string customerPhone;
    public string token;
    public string stage;

    public Dictionary<string, string> ConvertToParameters()
    {
        Dictionary<string, string> data = new Dictionary<string, string> ();
        Add (appId, APIHandler.APP_ID_KEY, APIHandler.APP_ID, data);
        Add (orderId, APIHandler.ORDER_ID_KEY, orderId, data);
        Add (orderAmount, APIHandler.ORDER_AMOUNT_KEY, orderAmount, data);
        Add (customerEmail, APIHandler.CUSTOMER_EMAIL_KEY, customerEmail, data);
        Add (customerPhone, APIHandler.CUSTOMER_PHONE_KEY, customerPhone, data);

        return data;
    }

    public void Add (string message, string key, string value, Dictionary<string, string> data)
    {
        if (!string.IsNullOrEmpty (message) && !data.ContainsKey(key))
        {
            data.Add (key, value);
        }
    }

}

