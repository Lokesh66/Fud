using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentRespose : MonoBehaviour
{
    public void onPaymentResponseRecieved(string s)
    {
        Debug.Log ("onPaymentResponseRecieved = " + s);
    }
}
