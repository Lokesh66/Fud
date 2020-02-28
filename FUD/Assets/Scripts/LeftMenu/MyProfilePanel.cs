using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyProfilePanel : MonoBehaviour
{
    public Image profileIcon;

    public TMP_Text nameText;
    public TMP_Text ageText;
    public TMP_Text emailText;
    public TMP_Text contactText;
    public TMP_Text roleText;
    public TMP_Text primarySkillsText;
    public TMP_Text secondarySkillsText;

    public void SetView()
    {
        UserData data = DataManager.Instance.userInfo;
        if (data != null)
        {
            nameText.text = data.name;
            ageText.text = data.dob;
            emailText.text = data.email_id;
            contactText.text = data.phone.ToString();
            roleText.text = data.role_id.ToString();
            primarySkillsText.text = "";
            secondarySkillsText.text = "";
        }
        gameObject.SetActive(true);
    }
    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
