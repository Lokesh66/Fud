using System.Collections.Generic;
using UnityEngine;


public class ProjectCastsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject castCell;
    public GameObject addNewButton;
    public NoDataView noData;

    public ProjectActivityPopUp statusPopUp;

    int projectId;
    List<ProjectCast> casts = new List<ProjectCast>();
    List<ProjectCharacter> characters = new List<ProjectCharacter>();


    private void OnEnable()
    {
        addNewButton.gameObject.SetActive(characters.Count > casts.Count);
    }

    private void OnDisable()
    {
        addNewButton.gameObject.SetActive(false);
    }

    public void SetData(int projectId, List<ProjectCast> casts)
    {
        this.projectId = projectId;

        this.casts = casts;

        parentContent.DestroyChildrens();

        if (casts != null && casts.Count > 0)
        {
            noData.gameObject.SetActive(false);

            for (int i = 0; i < casts.Count; i++)
            {
                GameObject auditionObject = Instantiate(castCell, parentContent);

                auditionObject.GetComponent<ProjectCastCell>().SetView(i, casts[i], OnCellTapAction);
            }
        }
        else
        {
            EnableNodata();
        }

        GameManager.Instance.apiHandler.GetProjectCharacters(projectId, (status, response) => {

            if (status)
            {
                ProjectCharactersResponse projectCharactersResponse = JsonUtility.FromJson<ProjectCharactersResponse>(response);
                characters = projectCharactersResponse.data;

                addNewButton.gameObject.SetActive(characters.Count > casts.Count);

                if(casts == null || casts.Count == 0)
                {
                    EnableNodata();
                }
            }
        });

    }


    public void CreateCast()
    {
        if (characters.Count > 0)
        {
            CreateCastView.Instance.SetView(projectId, characters, (isNewDataUpdated) =>
            {
                if (isNewDataUpdated)
                {
                    ProjectsDetailedView.Instance.Reload();
                }
            });
        }
    }

    void OnCellTapAction(ProjectCast projectCast)
    {
        statusPopUp.Load(projectCast, this);
    }

    public void OnStatusApplied(ProjectCast projectCast)
    {
        int modelIndex = casts.IndexOf(projectCast);

        parentContent.GetChild(modelIndex).GetComponent<ProjectCastCell>().SetView(modelIndex, projectCast, OnCellTapAction);
    }

    void EnableNodata()
    {
        noData.gameObject.SetActive(true);
        noData.SetView(GetNoDataModel());
    }


    NoDataModel GetNoDataModel()
    {
        NoDataModel model = new NoDataModel();
        model.buttonName = "Create Cast";
        model.subTitle = "No Cast Right now";
        if(characters.Count > casts.Count)
            model.buttonAction = CreateCast;
        return model;
    }
}
