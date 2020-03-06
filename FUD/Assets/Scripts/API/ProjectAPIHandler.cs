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

    public void CreateProject(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        /*Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("story_id", 1);

        parameters.Add("story_version_id", 1);

        parameters.Add("cost_estimation", long.Parse(budget));

        parameters.Add("estimated_time",  long.Parse(duration));*/

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetProjectStories(Action<bool, string> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_STORIES_FOR_CREATE_PROJECT, true, (status, response) =>
        {
            action(status, response);
        }));
    }

    public void GetProjectCharacters(int projectId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", projectId);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PROJECT_CHARACTERS, true, parameters, (status, response) =>
        {
            action(status, response);
        }));
    }

    public void CreateProjectScene(SceneCreationModel creationModel, List<Dictionary<string, object>> characterScenes, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", creationModel.project_id);

        parameters.Add("story_id", creationModel.story_id);

        parameters.Add("story_version_id", creationModel.story_version_id);

        parameters.Add("start_time", creationModel.start_time);

        parameters.Add("decsription", creationModel.decsription);

        parameters.Add("location", creationModel.location);

        parameters.Add("scene_order", creationModel.scene_order);

        parameters.Add("place_type", creationModel.place_type);

        parameters.Add("shoot_time", creationModel.shoot_time);

        parameters.Add("scene_characters", characterScenes);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_SCENE, true, parameters, (status, response) =>
        {
            action(status, response);
        }));
    }

    public void CreateProjectCast(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_CAST, true, parameters, (status, response) => {

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
    public List<SceneModel> StoryScenes;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class SceneModel
{
    public int id;
    public string place_type;
    public string shoot_time;
    public int story_id;
    public int scene_order;
    public int story_version_id;
    public int project_id;
    public string location;
    public DateTime start_time;
    public int status;
    public DateTime updatedAt;
}

[Serializable]
public class ProjectsResponse : BaseResponse
{
    public List<Project> data;
}


[Serializable]
public class ProjectCharacter
{
    public int id;
    public int story_id;
    public string title;
    public string description;
    public string suitable_performer;
    public object project_cast_id;
    public int project_id;
    public object estimated_working_days;
    public int project_status;
    public int story_version_id;
    public string gender;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public StoryVersions StoryVersions;
}

[Serializable]
public class ProjectCharactersResponse : BaseResponse
{
    public List<ProjectCharacter> data;
}

[Serializable]
public class SceneCharacter
{
    public int character_id = 1;
    public string dailogue = "Hello hero";
}

public class SceneCreationModel
{
    public int project_id;
    public int story_id;
    public int story_version_id;
    public string place_type;
    public string decsription;
    public string shoot_time;
    public int scene_order;
    public string location;
    public string start_time;
    public List<SceneCharacter> scene_characters;
}