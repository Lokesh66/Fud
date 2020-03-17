using System.Collections.Generic;
using UnityEngine;

public class UserAuditionController : MonoBehaviour
{

    public GameObject userAuditionCell;
    public Transform content;

    public GameObject buttonsPanel;

    UserAudition selectedAudition;

    AuditionController auditionController;
    public void SetView(AuditionController auditionController, List<UserAudition> auditions)
    {
        this.auditionController = auditionController;

        gameObject.SetActive(true);
        buttonsPanel.SetActive(false);
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < auditions.Count; i++)
        {
            GameObject storyObject = Instantiate(userAuditionCell, content);

            UserAuditionCell item = storyObject.GetComponent<UserAuditionCell>();

            item.SetView(auditions[i], OnAuditionSelectAction);
        }
    }

    void OnAuditionSelectAction(UserAudition audition)
    {
        Debug.Log("OnAuditionSelectAction : " + audition.user_id);
        selectedAudition = audition;
        buttonsPanel.SetActive(true);
    }

    void Reload()
    {
        gameObject.SetActive(false);
        auditionController.GetAuditions();
    }

    public void AcceptButtonAction()
    {
        buttonsPanel.SetActive(false);
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", selectedAudition.audition_id);
        parameters.Add("user_audition_id", selectedAudition.id);
        parameters.Add("status", "selected");
        GameManager.Instance.apiHandler.AcceptOrRejectAudition(parameters, (status, response) => {
            if (status)
            {
                Reload();
            }
        });
    }

    public void RejectButtonAction()
    {
        buttonsPanel.SetActive(false);
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", selectedAudition.audition_id);
        parameters.Add("user_audition_id", selectedAudition.id);
        parameters.Add("status", "rejected");
        GameManager.Instance.apiHandler.AcceptOrRejectAudition(parameters, (status, response) => {
            if (status)
            {
                Reload();
            }
        });
    }
}
