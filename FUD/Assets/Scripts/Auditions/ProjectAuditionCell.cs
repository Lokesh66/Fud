using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProjectAuditionCell : MonoBehaviour
{
    public RawImage auditionIcon;

    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text peopleJoinedText;
    public TMP_Text projectNameText;

    Audition auditionData;


    public void SetView(Audition audition)
    {
        auditionData = audition;

        if (auditionData != null)
        {
            titleText.text = auditionData.topic;

            descriptionText.text = auditionData.description;

            peopleJoinedText.text = "Entries : " + auditionData.no_of_persons_joined.ToString();

            projectNameText.text = auditionData.title;

            SetAuditionIcon();
        }
    }

    void SetAuditionIcon()
    {
        GameManager.Instance.apiHandler.DownloadImage(auditionData.image_url, (sprite) =>
        {
            if (this != null && sprite != null)
            {
                auditionIcon.texture = sprite.texture;
            }
        });
    }

    public void OnClickAction()
    {
        ProjectsDetailedView.Instance.auditionDetails.Load(auditionData, (index) => {

            switch (index)
            {
                case 0: //Edit
                    CreateAuditionView.Instance.EditView(auditionData, Refresh);
                    break;
                case 1: //Delete

                    DeleteAudition();
                    break;
                case 2: //Close
                    break;

                case 3: //View
                    ShowActiveAuditions();
                    break;
            }
        });
    }

    void Refresh(bool isUpdated)
    {
        if (isUpdated)
        {
            ProjectsDetailedView.Instance.Reload();
        }

    }

    void UpdateList()
    {
        Refresh(true);
    }

    void DeleteAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", auditionData.id);

        parameters.Add("status",8);

        GameManager.Instance.apiHandler.ModifyAudition(parameters, (status, response) => {

            if (status)
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Deleted Successfully";
                alertModel.okayButtonAction = UpdateList;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Deleting Audition Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void ShowActiveAuditions()
    {
        if (auditionData.no_of_persons_joined <= 0)
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = "You have not received any audition responses";

            UIManager.Instance.ShowAlert(alertModel);

            return;
        }


        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("id", auditionData.id);
        parameters.Add("page", 0);
        parameters.Add("limit", 20);

        GameManager.Instance.apiHandler.SearchAuditions(1, parameters, (status, response) => {

            if (status)
            {
                SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                ProjectsDetailedView.Instance.userAuditionController.SetView(auditionResponse.data, auditionData.id, null);
            }
        });
    }
}
