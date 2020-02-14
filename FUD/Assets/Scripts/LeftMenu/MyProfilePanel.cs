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
        gameObject.SetActive(true);
    }
    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
