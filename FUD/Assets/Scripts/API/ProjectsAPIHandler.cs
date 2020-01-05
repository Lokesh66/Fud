using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

public partial class APIHandler
{
    public void GetAllProjects(Action<List<Project>> action)
    {
        GetRequest(APIConstants.CREATE_PROJECT, (bool status, string response) => {

            ProjectResponse projectResponse = JsonUtility.FromJson<ProjectResponse>(response);
            Debug.Log("Get projects : "+projectResponse.data.Count);
            action?.Invoke(projectResponse.data);

        });
    }

    public void CreateProject(Project project, Action<Project> action)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        parameters.Add("title", project.title);
        parameters.Add("story_id", project.story_id.ToString());
        parameters.Add("cost_estimation", project.cost_estimation.ToString());
        parameters.Add("estimated_time", project.estimated_time.ToString());

        /*"title": "rakshak",
		"story_id": 1,
		"cost_estimation" : 1000000,
		"estimated_time": 1609818627*/

        PostRequest(APIConstants.CREATE_PROJECT, parameters, (bool status, string response) => {

            ProjectCreatedResponse project1 = JsonUtility.FromJson<ProjectCreatedResponse>(response);
            action?.Invoke(project1.data);
        });
    }
}

[Serializable]
public class Project
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
public class ProjectResponse
{
    public string message;
    public List<Project> data;
    public int status;
}

[Serializable]
public class ProjectCreatedResponse
{
    public string message;
    public Project data;
    public int status;
}
