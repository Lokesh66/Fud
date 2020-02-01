using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryVersionsView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;


    List<StoryVersion> storyVersionList;


    public void EnableView(List<StoryVersion> versionsList)
    {
        gameObject.SetActive(true);

        if (storyVersionList?.Count > 0)
        {
           
        }
        else {
            storyVersionList = versionsList;

            SetView();
        }
    }

    void SetView()
    {
        for (int i = 0; i < storyVersionList.Count; i++)
        {
            GameObject storyObject = Instantiate(storyCell, content);

            storyObject.GetComponent<StoryVersionCell>().SetView(storyVersionList[i]);
        }
    }

    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        storyVersionList.Clear();
    }
}
