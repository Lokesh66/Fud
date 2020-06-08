using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentRespose : MonoBehaviour
{
    public void onPaymentResponseRecieved(string status, string orderId)
    {
        Debug.Log ("onPaymentResponseRecieved = " + status + " -- " + orderId);
        AlertMessage.Instance.SetText("onPaymentResponseRecieved : "+ status + " -- " + orderId, false);

        
        switch (status)
        {
            case "success":
                Loader.Instance.StartLoading();
                GameManager.Instance.apiHandler.VerifyPurchsedOrderId(orderId, (apiStatus) =>
                {
                    Loader.Instance.StopLoading();
                    if (apiStatus)
                    {
                        AlertModel alertModel = new AlertModel();

                        alertModel.message = "Order Verified Success";

                        UIManager.Instance.ShowAlert(alertModel);
                    }
                    else
                    {
                        AlertModel alertModel = new AlertModel();

                        alertModel.message = "Order verification Failed";

                        UIManager.Instance.ShowAlert(alertModel);
                    }
                });
                break;
            default:
                break;
        }
        
    }
}
