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

    public TextMeshProUGUI title;

    public TextMeshProUGUI description;

    public Image icon;

    public GameObject joinButton;

    public TMP_Text joinButtonText;

    public GameObject withdrawButton;

    System.Action<int> buttonAction;
    public void Load(Audition audition,bool isJoined, System.Action<int> action)
    {
        parentPanel.SetActive(true);
        //joinButton.SetActive(!isJoined);
        joinButtonText.text = isJoined ? "Update" : "Join";
        withdrawButton.SetActive(isJoined);
        title.text = audition.title;
        description.text = audition.description;
        GameManager.Instance.downLoadManager.DownloadImage(audition.image_url, (sprite) => {
            if (sprite != null)
            {
                icon.sprite = sprite;
            }
        });
        buttonAction = action;
    }

    public void OnButtonAction(int buttonIndex)
    {
        parentPanel.SetActive(false);

        buttonAction?.Invoke(buttonIndex);
    }
}
