using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditionCreatedDetailsView : MonoBehaviour
{

    AuditionCreatedCell auditionCell;

    Audition auditionModel;

    AuditionCreatedView controller;


    public void Load(AuditionCreatedView controller, AuditionCreatedCell activityModel)
    {
        this.auditionCell = activityModel;

        this.controller = controller;

        this.auditionModel = activityModel.GetAuditionModel();

        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        Audition audition = auditionCell.GetAuditionModel();

        if (audition.no_of_persons_joined <= 0)
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = "You have not received any audition responses";

            UIManager.Instance.ShowAlert(alertModel);

            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", auditionModel.id);

        parameters.Add("source_from", 0);


        GameManager.Instance.apiHandler.SearchAuditions(1, parameters, (status, response) =>
        {
            if (status)
            {
                Debug.Log(response);

                SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                controller.SetUserAuditions(auditionResponse.data, auditionModel.id);
            }
        });
    }

    public void OnEditButtonAction()
    {
        gameObject.SetActive(false);

        CreateAuditionView.Instance.EditView(auditionModel, Refresh);
    }

    void Refresh(bool isDataUpdated)
    {
        if (isDataUpdated)
        {
            controller.GetAuditions();
        }
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }
}
