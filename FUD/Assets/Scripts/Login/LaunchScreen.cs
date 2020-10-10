using UnityEngine;

public enum ELoginButtonType
{
    None,
    Login,
    SignUp
}
public class LaunchScreen : MonoBehaviour
{
    System.Action<ELoginButtonType> OnButtonPressed;

    public void SetView(System.Action<ELoginButtonType> action)
    {
        gameObject.SetActive(true);
        OnButtonPressed = action;
    }

    public void OnClick_SignIn()
    {
        OnButtonPressed?.Invoke(ELoginButtonType.SignUp);
        OnButtonPressed = null;
    }

    public void OnClick_Login()
    {
        OnButtonPressed?.Invoke(ELoginButtonType.Login);
        OnButtonPressed = null;
    }
}
