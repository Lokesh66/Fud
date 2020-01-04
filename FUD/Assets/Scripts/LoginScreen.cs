using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    public TMP_InputField inputField;
    public RoleSelection roleSelection;
    private void OnEnable()
    {
        inputField.text = "";
    }

    public void OnClickSendOTP()
    {
        GameManager.Instance.apiHandler.SendOTP(inputField.text, (bool status) => {

            roleSelection.SetView((CraftRoleItem item) => {
                Debug.Log(item.nameText.text);
            });
        });
    }
}
