using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuditionJoinView : MonoBehaviour
{
    #region Singleton

    private static AuditionJoinView instance = null;

    private AuditionJoinView()
    {

    }

    public static AuditionJoinView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AuditionJoinView>();
            }
            return instance;
        }
    }
    #endregion

    public GameObject parentPanel;

    public GameObject joinButton;

    public GameObject acceptObject;

    public GameObject shortListObject;

    public TMP_Text joinButtonText;

    public GameObject withdrawButton;

    public GameObject recordObject;


    System.Action<int> buttonAction;


    public void Load(Audition audition,bool isJoined, System.Action<int> action, EAuditionStatus auditionStatus = EAuditionStatus.None)
    {
        Debug.LogError("Audition Detail Page Loaded");

        parentPanel.SetActive(true);
        //joinButton.SetActive(!isJoined);
        joinButtonText.text = isJoined ? "Update" : "Join";
        withdrawButton.SetActive(isJoined);
        recordObject.SetActive(!isJoined);
        acceptObject.SetActive(isJoined);
        shortListObject.SetActive(isJoined);
        buttonAction = action;
    }

    public void OnButtonAction(int buttonIndex)
    {
        parentPanel.SetActive(false);

        buttonAction?.Invoke(buttonIndex);
    }
}
