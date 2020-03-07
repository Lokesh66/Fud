using UnityEngine;
using TMPro;

public class AuditionDetailsView : MonoBehaviour
{
    public TextMeshProUGUI description;

    System.Action<int> buttonAction;

    public void Load(Audition audition, System.Action<int> action)
    {
        gameObject.SetActive(true);

        description.text = audition.description;

        buttonAction = action;
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        buttonAction?.Invoke(buttonIndex);
    }
}
