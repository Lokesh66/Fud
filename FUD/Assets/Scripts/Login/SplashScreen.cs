using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    Firebase.FirebaseApp app;

    private void Start()
    {
        Invoke("GoToLaunchScreen", 2.0f);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void GoToLaunchScreen()
    {
        GameManager.Instance.sceneController.SwitchScene(ESceneType.LoginScene);
    }
}
