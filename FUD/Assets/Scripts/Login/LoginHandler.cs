using UnityEngine;

enum ELoginFlow
{
    None,
    Launch,
    RoleSelection,
    Details,
    Login
}

public class LoginHandler : MonoBehaviour
{
    /*#region Singleton
    private static LoginHandler instance = null;

    private LoginHandler()
    {

    }

    public static LoginHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoginHandler>();
            }
            return instance;
        }
    }
    #endregion*/

    public LaunchScreen launchScreen;
    public RoleSelectionScreen roleSelectionScreen;
    public DetailsScreen detailsScreen;
    public LoginScreen loginScreen;

    Craft selectedRole;
    string contactNumber;
    User user;
    bool isNewUser = true;

    private void Start()
    {
        SetView(ELoginFlow.Launch);
    }

    private void SetView(ELoginFlow eLoginFlow)
    {
        switch (eLoginFlow)
        {
            case ELoginFlow.Launch:
                launchScreen.SetView(OnLaunchScreen_CallBack);
                break;
            case ELoginFlow.RoleSelection:
                roleSelectionScreen.SetView(OnRoleSelection_CallBack);
                break;
            case ELoginFlow.Details:
                detailsScreen.SetView(OnDetailsScreen_CallBack);
                break;
            case ELoginFlow.Login:
                loginScreen.SetView(contactNumber, OnLoginScreen_CallBack);
                break;
        }
    }

    void OnLaunchScreen_CallBack(ELoginButtonType loginType)
    {
        launchScreen.gameObject.SetActive(false);
        switch (loginType) {
            case ELoginButtonType.Login:
                isNewUser = false;
                SetView(ELoginFlow.Details);
                break;

            case ELoginButtonType.SignUp:
                isNewUser = true;
                SetView(ELoginFlow.RoleSelection);
                break;
        }
    }

    void OnRoleSelection_CallBack(Craft item)
    {
        roleSelectionScreen.gameObject.SetActive(false);

        if (item == null)
        {
            SetView(ELoginFlow.Launch);
        }
        else
        {
            selectedRole = item;
            SetView(ELoginFlow.Details);
        }
    }

    void OnDetailsScreen_CallBack(bool status, string mobileNumber)
    {
        detailsScreen.gameObject.SetActive(false);

        if (status)
        {
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                contactNumber = mobileNumber;
            }
            SetView(ELoginFlow.Login);
        }
        else
        {
            if (isNewUser)
            {
                SetView(ELoginFlow.RoleSelection);
            }
            else
            {
                SetView(ELoginFlow.Launch);
            }
        }
    }

    void OnLoginScreen_CallBack(bool status, User user)
    {
        loginScreen.gameObject.SetActive(false);

        if (status)
        {
            if(user != null)
            {
                this.user = user;
            }
            GameManager.Instance.sceneController.SwitchScene(ESceneType.HomeScene);
        }
        else
        {
            SetView(ELoginFlow.Details);
        }
    }

    public void OnClickCreateNewAccount()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickLogin()
    {
        this.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(true);
    }

}
