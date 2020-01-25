using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetProjects(Action<bool, AllProjectsResponse> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_PROJECT, true, (status, response) => {

            if (status)
            {
                AllProjectsResponse projectsResponse = JsonUtility.FromJson<AllProjectsResponse>(response);

                action(status, projectsResponse);
            }
        }));
    }

    public void UpdateProjectDetails(string title, string budget, string duration, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("story_id", 1);

        parameters.Add("cost_estimation", long.Parse(budget));

        parameters.Add("estimated_time",  long.Parse(duration));

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT, true, parameters, (status, response) => {

            action(status, response);
        }));
    }
}

[Serializable]
public class BaseResponse
{
    public int status;
    public string message;
}

[Serializable]
public class ProjectDataModel
{
    public int id;
    public int story_id;
    public int user_id;
    public string title;
    public int cost_estimation;
    public int estimated_time;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class ProjectResponse : BaseResponse
{
    public ProjectDataModel data;
}

[Serializable]
public class AllProjectsResponse : BaseResponse
{
    public List<ProjectDataModel> data;
}
