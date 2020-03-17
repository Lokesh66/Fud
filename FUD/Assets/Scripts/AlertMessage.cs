using UnityEngine;
using UnityEngine.UI;

public class AlertMessage : MonoBehaviour
{
    private static AlertMessage instance = null;

    private AlertMessage()
    {

    }

    public static AlertMessage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AlertMessage>();
            }
            return instance;
        }
    }

    public Text message;

    // Start is called before the first frame update
    public void SetText(string val, bool append = true)
    {
        if (append)
        {
            message.text += "\n" + val;
        }
        else
        {
            message.text = val;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
