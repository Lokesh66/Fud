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
            roleSelection.SetView((CraftRoleItem selectedItem) => {
                roleSelection.gameObject.SetActive(false);
                GameManager.Instance.sceneController.SwitchScene(ESceneType.HomeScene);

            });
        });
    }
}
