using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentResposeHelper : MonoBehaviour
{
    AndroidJavaClass ajc;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ajc = new AndroidJavaClass("com.phonatoto.cashfree.PaymentActivity");
        if (ajc != null)
            Debug.Log("Hello called");
        //SendToAndroid("Hello World");
    }
    private void SendToAndroid(string message)
    {
        ajc.CallStatic("ShowToast", message);
    }
    // This function will be called from Android
    private void onPaymentResponseRecieved(string txStatus, string referenceId)
    {
        Debug.Log("Received message from plugin : txStatus : " + txStatus + " referenceId : " + referenceId);
    }
}