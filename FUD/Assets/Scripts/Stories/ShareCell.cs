using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ShareCell : MonoBehaviour
{
    public TextMeshProUGUI userName;

    public Image profileImage;


    StoryVersion storyVersion;


    public void Init(StoryVersion storyVersion)
    {
        this.storyVersion = storyVersion;
    }

    public void OnPostAction()
    { 
        
    }
}
