using System.Runtime.InteropServices;
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
    public const string NOTIFY_URL_KEY = "notifyUrl";

    public const string APP_ID = "1362262b24f262b46ab91599e22631";


#if UNITY_IOS

    private delegate void OnIOSPaymentCompletion(string result);

    [DllImport("__Internal")]
    private static extern float processPayment(string paymentInfo, OnIOSPaymentCompletion onPaymentCompletion);
#endif


    public void Example()
    {
        Dictionary<string, object> planIdInfo = new Dictionary<string, object>
        {
            { "plan_id", 1 }
        };
        CashFreeRequest ("911234567890", "GMAIL_ACCOUNT", planIdInfo);
    }

    public void CashFreeRequest (string customerPhone, string customerEmail, Dictionary<string, object>  planIdInfo, string customerName = null, string orderNote = null, Action<bool, string> OnResponse = null)
    {
        Debug.Log ("CashFree CashFreeRequest");

        gameManager.StartCoroutine(gameManager.apiHandler.PostRequest (APIConstants.GET_CASH_FREE_TOKEN, true, planIdInfo, (bool status, string responseString) => 
        {
            Debug.Log ("CashFree CashFreeRequest status = " + status);

            Debug.Log("Response string = " + responseString);

            //"data":{ "appId":"1362262b24f262b46ab91599e22631","orderId":8,"orderAmount":1000,"customerEmail":"lmoakm@gmail.com","customerPhone":919133532445,"token":"lb9JCN4MzUIJiOicGbhJCLiQ1VKJiOiAXe0Jye.gV0nIkZWZkRTZhF2MlZTZ1IiOiQHbhN3XiwiN4IDN3gjN4UTM6ICc4VmIsIiUOlkI6ISej5WZyJXdDJXZkJ3biwCMwATM6ICduV3btFkclRmcvJCL4ojIklkclRmcvJye.jmlUm863f1eFUTAx6dPOwjB0vIUXbx2HKcIIUFH-eLbhnkAc-JWG8KDiHt7hFck1x7","stage":"TEST","notifyUrl":"http://18.217.51.190:7004/v1/verify/order"}
            //})

            if (status)
            {
                
                CFPToken response = JsonUtility.FromJson<CFPTokenResponse> (responseString).data;

#if UNITY_ANDROID
                Dictionary<string, string> parameters = response.ConvertToParameters ();

                response.Add (APP_ID, APP_ID_KEY, APP_ID, parameters);
                response.Add (customerPhone, CUSTOMER_PHONE_KEY, customerPhone, parameters);
                response.Add (customerEmail, CUSTOMER_EMAIL_KEY, customerEmail, parameters);

                response.Add (orderNote, ORDER_NOTE_KEY, orderNote, parameters);            // Optional
                response.Add (customerName, CUSTOMER_NAME_KEY, customerName, parameters);   // Optional
                response.Add (response.notifyUrl, CUSTOMER_NAME_KEY, response.notifyUrl, parameters);   // Optional
                //response.Add(response.returnUrl, CUSTOMER_NAME_KEY, response.returnUrl, parameters);   // Optional


                AndroidJavaObject paramObj = CreateJavaMapFromDictainary (parameters);

                DataManager.Instance.SetPurchaseOrderId(response.orderId);

                if (string.IsNullOrEmpty (response.tokenData))
                { 
                    response.tokenData = GetCFPToken ();
                }

                //if (string.IsNullOrEmpty(response.))
                //{
                //    response.stage = "TEST";
                //}
                AndroidJavaObject currentActivity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject> ("currentActivity");

                AndroidJavaClass paymentActivity = new AndroidJavaClass ("com.phonatoto.cashfree.PaymentActivity");

                paymentActivity.CallStatic ("doPayment", currentActivity, paramObj, response.tokenData, "TEST", new AndroidIAPCallBack());

                //OnResponse(status, response.orderId);

#else
                //TODO: iOS is pending

                Debug.Log("iOS Cash free implementation is pending");

                string jsonResponse = JsonUtility.ToJson(response);

                Debug.Log("jsonResponse = " + jsonResponse);

                processPayment(jsonResponse, onPaymentCompletion);
#endif
    }
        }));
    }

    //[AOT.MonoPInvokeCallback(typeof(OnIOSPaymentCompletion))]
    static void onPaymentCompletion(string response)
    {
        Debug.Log("response = " + response);

        PaymentCompletionModel responseModel = JsonUtility.FromJson<PaymentCompletionModel>(response);

        if (responseModel.txStatus.Equals("SUCCESS"))
        {
            GameManager.Instance.apiHandler.VerifyPurchsedOrderId(responseModel.orderId, (apiStatus) =>
            {
                if (apiStatus)
                {
                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "Order Verified Success";

                    UIManager.Instance.ShowAlert(alertModel);

                    DataManager.Instance.ClearPurchaseOrderId();
                }
                else
                {
                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "Order verification Failed";

                    UIManager.Instance.ShowAlert(alertModel);
                }
            });
        }
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
        return null;

    }
}

[Serializable]
public class CFPTokenResponse : BaseResponse
{
    public CFPToken data;
}
[Serializable]
public class CFPToken
{
    public string appId;
    public string orderId;
    public string orderAmount;
    public string customerEmail;
    public string customerPhone;
    public string customerName;
    public string orderCurrency;
    public string tokenData;
    public string orderNote;
    //public string stage;
    public string notifyUrl;

    public Dictionary<string, string> ConvertToParameters()
    {
        Dictionary<string, string> data = new Dictionary<string, string> ();
        Add (appId, APIHandler.APP_ID_KEY, APIHandler.APP_ID, data);
        Add (orderId.ToString(), APIHandler.ORDER_ID_KEY, orderId.ToString(), data);
        Add (orderAmount.ToString(), APIHandler.ORDER_AMOUNT_KEY, orderAmount.ToString(), data);
        Add (customerEmail.ToString(), APIHandler.CUSTOMER_EMAIL_KEY, customerEmail.ToString(), data);
        Add (customerPhone.ToString(), APIHandler.CUSTOMER_PHONE_KEY, customerPhone.ToString(), data);

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

class AndroidIAPCallBack : AndroidJavaProxy
{
    public AndroidIAPCallBack() : base("com.phonatoto.cashfree.PaymentResponseCallBack") { }

    public void onSuccess(string message)
    {
        string orderId = DataManager.Instance.GetPurchaseOrderId();

        GameManager.Instance.apiHandler.VerifyPurchsedOrderId(orderId, (apiStatus) =>
        {
            if (apiStatus)
            {
                AlertModel alertModel = new AlertModel();

                alertModel.message = "Order Verified Success";

                UIManager.Instance.ShowAlert(alertModel);

                DataManager.Instance.ClearPurchaseOrderId();
            }
            else
            {
                AlertModel alertModel = new AlertModel();

                alertModel.message = "Order verification Failed";

                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    public void onFailure(string errorMessage)
    {
        DataManager.Instance.ClearPurchaseOrderId();
    }
}

[Serializable]
public class PaymentCompletionModel
{
    public string orderId;
    public string referenceId;
    public string orderAmount;
    public string txStatus;
    public string txMsg;
    public string txTime;
    public string paymentMode;
    public string signature;
}

public interface PaymentResponseCallBack
{
    void onSuccess(String transactionRefId);
    void onFailure(String transactionRefId);
}

