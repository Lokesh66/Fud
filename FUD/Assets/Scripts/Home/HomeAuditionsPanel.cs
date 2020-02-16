using System.Collections.Generic;
using UnityEngine;

public class HomeAuditionsPanel : MonoBehaviour
{
    public GameObject homeCell;
    public Transform parentContent;
    List<Audition> auditionsList = new List<Audition>();

    private void OnEnable()
    {
        GetAuditions();
    }

    void GetAuditions()
    {
        auditionsList = new List<Audition>();
        GameManager.Instance.apiHandler.SearchAuditions(false, (status, auditionsList) => {

            if (status)
            {
                this.auditionsList = auditionsList;
            }

            SetView();
        });
    }

    void SetView()
    {
        foreach (Transform child in parentContent)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (auditionsList == null)
        {
            auditionsList = new List<Audition>();
        }

        for(int i = 0; i < auditionsList.Count; i++)
        {
            GameObject storyObject = Instantiate(homeCell, parentContent);

            HomeCell homeItem = storyObject.GetComponent<HomeCell>();

            homeItem.SetView(HomeCellType.AUDITION, i, auditionsList[i].title, OnAuditionSelectAction);
        }
    }

    void OnAuditionSelectAction(int index)
    {
        //Open audition of storieslist[index]

        Debug.Log("OnAuditionSelectAction : "+index);
    }
}
