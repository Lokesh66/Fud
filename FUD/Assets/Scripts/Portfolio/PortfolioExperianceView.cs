using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioExperianceView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;

    public PortfolioExperienceDetails experienceDetails;


    private PortfolioView portfolioView;

    List<WorkExperianceModel> experianceModels;

    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        SetExperianceModels();
    }

    void SetExperianceModels()
    {
        content.DestroyChildrens();

        GameManager.Instance.apiHandler.GetAllExperiances((status, modelsList) => {

            if (status)
            {
                experianceModels = modelsList;

                SetView();
            }
        });
    }

    void SetView()
    {
        for (int i = 0; i < experianceModels.Count; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            cellObject.GetComponent<ExperianceCell>().SetView(experianceModels[i], OnCellButtonAction);
        }
    }

    void OnCellButtonAction(WorkExperianceModel experianceModel)
    {
        experienceDetails.Load(experianceModel, this);
    }

    public void RemovePortfolioExperience(WorkExperianceModel experienceModel)
    {
        int modelIndex = experianceModels.IndexOf(experienceModel);

        Destroy(content.GetChild(modelIndex).gameObject);

        experianceModels.Remove(experienceModel);
    }
}
