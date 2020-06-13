using UnityEngine;
using TMPro;

public class ProjectAuditionDetailView : MonoBehaviour
{
    public TextMeshProUGUI description;

    public GameObject editObject;

    System.Action<int> buttonAction;


    public void Load(Audition audition, System.Action<int> action)
    {
        gameObject.SetActive(true);

        description.text = audition.description;

        buttonAction = action;

        bool isAuditionOwner = DataManager.Instance.userInfo.id == audition.user_id;

        editObject.SetActive(isAuditionOwner);
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        buttonAction?.Invoke(buttonIndex);
    }
}
