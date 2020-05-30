using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentResposeHelper : MonoBehaviour
{
    AndroidJavaClass ajc;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ajc = new AndroidJavaClass("phonatoto.cashfree.PaymentActivity");
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
