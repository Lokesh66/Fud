using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetProjects(Action<bool, ProjectsResponse> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_PROJECT, true, (status, response) => {

            if (status)
            {
                ProjectsResponse projectsResponse = JsonUtility.FromJson<ProjectsResponse>(response);

                action(status, projectsResponse);
            }
        }));
    }

    public void GetProjectDetails(int id, Action<bool, Project> action)
    {
        string url = APIConstants.GET_PROJECT_DETAILS + id;

        gameManager.StartCoroutine(GetRequest(url, true, (status, response) =>
        {
            if (status)
            {
                ProjectsResponse projectsResponse = JsonUtility.FromJson<ProjectsResponse>(response);
                if (projectsResponse.data != null && projectsResponse.data.Count > 0)
                {
                    action(status, projectsResponse.data[0]);
                }
                else
                {
                    action(status, null);
                }
            }
            else
            {
                action(status, null);
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

    public void CreateProjectCast(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        parameters.Add("project_id", 1);

        parameters.Add("story_character_id", 2);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_CAST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ModifyProjectCast(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        parameters.Add("id", 1);

        parameters.Add("project_id", 1);

        parameters.Add("story_character_id", 2);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT_CAST, true, parameters, (status, response) => {

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
public class ProjectCast
{
    public int id;
    public int project_id;
    public int story_character_id;
    public object selected_member;
    public int user_id;
    public int status;
    public int cast_status;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class Project
{
    public int id;
    public int story_id;
    public int story_version_id;
    public int user_id;
    public string title;
    public int cost_estimation;
    public int estimated_time;
    public List<StoryVersion> StoryVersions;
    public List<ProjectCast> Project_cast;
    public List<Audition> Audition;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class ProjectsResponse : BaseResponse
{
    public List<Project> data;
}
