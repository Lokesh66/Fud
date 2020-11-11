using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetProjects(int pageNo, Action<bool, ProjectsResponse> action)
    {
        string url = APIConstants.CREATE_PROJECT;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(GetRequest(url, true, (status, response) => {

            Debug.Log("GetProjects : response = " + response);

            if (status)
            {
                ProjectsResponse projectsResponse = JsonUtility.FromJson<ProjectsResponse>(response);

                action(status, projectsResponse);
            }
        }));
    }

    public void GetUserInfo(int userId, Action<bool, UserData> action)
    {
        string url = APIConstants.GET_USER_INFO + "?user_id=" + userId;

        gameManager.StartCoroutine(GetRequest(url, true, (status, response) => {

            Debug.Log("GetUserInfo : response = " + response);

            if (status)
            {
                UserData responseModel = JsonUtility.FromJson<UserDetailResponse>(response).data;

                action(status, responseModel);
            }
        }));
    }

    public void GetOfferedProjects(int pageNo, Action<bool, ProjectOfferedResponse> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_OFFERED_PROJECTS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("tab_name", "offeres");

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            ProjectOfferedResponse projectsResponse = JsonUtility.FromJson<ProjectOfferedResponse>(response);

            action(status, projectsResponse);
        }));
    }

    public void GetAlteredProjects(int pageNo, Action<bool, ProjectOfferedResponse> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_PROJECTS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("tab_name", "altered");

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            ProjectOfferedResponse projectsResponse = JsonUtility.FromJson<ProjectOfferedResponse>(response);

            action(status, projectsResponse);
        }));
    }

    public void GetProjectProducers(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_PRODUCERS_LIST;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ApplyOfferedProjectsFilter(int sortId, int roleId, int orderBy, int producerId, Action<bool, ProjectOfferedResponse> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_OFFERED_PROJECTS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        if (!roleId.Equals(-1))
        {
            parameters.Add("role_id", roleId);
        }

        if (!sortId.Equals(-1))
        {
            parameters.Add("sortBy", sortId);
        }

        if (!orderBy.Equals(-1))
        {
            parameters.Add("sortOrder", orderBy);
        }

        if (!producerId.Equals(-1))
        {
            parameters.Add("producer_id", producerId);
        }

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            ProjectOfferedResponse projectsResponse = JsonUtility.FromJson<ProjectOfferedResponse>(response);

            action(status, projectsResponse);
        }));
    }

    public void ApplyAlteredProjectsFilter(int statusId, int sortId, int orderId, Action<bool, ProjectsResponse> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_PROJECTS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        if (!sortId.Equals(-1))
        {
            parameters.Add("sortBy", sortId);
        }
        if (!statusId.Equals(-1))
        {
            parameters.Add("status", statusId);
        }
        if (!orderId.Equals(-1))
        {
            parameters.Add("sortOrder", orderId);
        }

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            ProjectsResponse projectsResponse = JsonUtility.FromJson<ProjectsResponse>(response);

            action(status, projectsResponse);
        }));
    }

    public void GetBrowserData(int pageNo, Action<bool, List<PortfolioModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_PROJECT_BROWSER_DATA;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            ProjectBrowserResponse projectsResponse = JsonUtility.FromJson<ProjectBrowserResponse>(response);

            if (status)
            {
                action(status, projectsResponse.data);
            }
            else {
                CreateAlert(projectsResponse.message);
            }
        }));
    }

    public void GetProjectDetails(int id, Action<bool, Project> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", id);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PROJECT_DETAILS, true, parameters, (status, response) =>
        {
            if (status)
            {
                Debug.Log("GetProjectDetails : response = " + response);

                ProjectDetailResponse projectsResponse = JsonUtility.FromJson<ProjectDetailResponse>(response);

                if (projectsResponse.data != null)
                {
                    action(status, projectsResponse.data);
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
        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProject(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProjectStauts(int projectId, int castId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_cast_id", castId);

        parameters.Add("project_id", projectId);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_OFFERED_PROJECT_STATUS, true, parameters, (apiStatus, response) =>
        {
            action(apiStatus, response);
        }));
    }

    public void ShareProjectScene(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PutRequest(APIConstants.SHARE_SCENE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProjectCastStauts(int projectId, int projectCastId, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", projectId);

        parameters.Add("project_cast_id", projectCastId);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_PROJECT_CAST_STATUS, true, parameters, (apiStatus, response) =>
        {
            action(apiStatus);
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

    public void CreateProjectScene(SceneCreationModel creationModel, List<Dictionary<string, int>> characterScenes, List<Dictionary<string, object>> autoScenes, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", creationModel.project_id);

        parameters.Add("story_id", creationModel.story_id);

        parameters.Add("story_version_id", creationModel.story_version_id);

        parameters.Add("start_time", creationModel.start_time);

        parameters.Add("description", creationModel.decsription);

        parameters.Add("location", creationModel.location);

        parameters.Add("scene_order", creationModel.scene_order);

        parameters.Add("place_type", creationModel.place_type);

        parameters.Add("shoot_time", creationModel.shoot_time);

        if (characterScenes.Count > 0)
        {
            parameters.Add("scene_characters", characterScenes);
        }

        if (autoScenes.Count > 0)
        {
            parameters.Add("scene_multi_media", autoScenes);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_SCENE, true, parameters, (status, response) =>
        {
            action(status, response);
        }));
    }

    public void CreateProjectTeam(int project_id, string title, string description, string members, int accessValue, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", project_id);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("members", members);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_TEAM, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProjectTeam(int project_id, string title, int teamId, string description, string members, int accessValue, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", project_id);

        parameters.Add("id", teamId);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("members", members);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT_TEAM, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemoveProjectTeam(int teamId, int projectId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", projectId);

        parameters.Add("id", teamId);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT_TEAM, true, parameters, (apiStatus, response) => {

            action(apiStatus, response);
        }));
    }

    public void UpdateProjectScene(SceneCreationModel creationModel, int sceneId, List<Dictionary<string, int>> sceneCharacters, List<Dictionary<string, object>> autoDialogues, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", sceneId);

        parameters.Add("project_id", creationModel.project_id);

        parameters.Add("story_id", creationModel.story_id);

        parameters.Add("story_version_id", creationModel.story_version_id);

        parameters.Add("start_time", System.DateTime.Now);//creationModel.start_time);

        parameters.Add("decsription", creationModel.decsription);

        parameters.Add("location", creationModel.location);

        parameters.Add("scene_order", creationModel.scene_order);

        parameters.Add("place_type", creationModel.place_type);

        parameters.Add("shoot_time", creationModel.shoot_time);

        if (sceneCharacters.Count > 0)
        {
            parameters.Add("scene_characters", sceneCharacters);
        }

        if (autoDialogues.Count > 0)
        {
            parameters.Add("scene_multi_media", autoDialogues);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT_SCENE, true, parameters, (status, response) =>
        {
            action(status, response);
        }));
    }

    public void GetSceneMembers(int projectId, Action<bool, List<SceneAlbumModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("project_id", projectId);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_SCENE_MEMBERS, true, parameters, (status, response) =>
        {
            if (status)
            {
                SceneCharacterAlbumResponse responseModel = JsonUtility.FromJson<SceneCharacterAlbumResponse>(response);

                action(status, responseModel.data);
            }
        }));
    }

    public void CreateProjectCast(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_PROJECT_CAST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetSceneDetails(int id, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_SCENE_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemoveProjectScene(int id, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_PROJECT_SCENE, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void ApplyBrowseFilter(int ageFrom, int ageTo, string gender, int roleId, Action<bool, List<PortfolioModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("age_from", ageFrom);

        parameters.Add("age_to", ageTo);

        parameters.Add("role_id", roleId);

        parameters.Add("gender", gender);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PROJECT_BROWSER_DATA, true, parameters, (apiStatus, response) =>
        {
            ProjectBrowserResponse projectsResponse = JsonUtility.FromJson<ProjectBrowserResponse>(response);

            action(apiStatus, projectsResponse.data);

        }));
    }

    public void UpdateShortListedAlbums(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_SHORTLISTED_ALBUMS, true, parameters, (status, response) => {

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
    public int selected_member;
    public int user_id;
    public int status;
    public int cast_status;
    public int payment_status;
    public int payment_type;
    public int recurring_payment;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public ProjectCharacterModel StoryCharacters;
}

[Serializable]
public class Project
{
    public int id;
    public int story_id;
    public int story_version_id;
    public int user_id;
    public string title;
    public string description;
    public int cost_estimation;
    public int estimated_time;
    public int crew_percentage;
    public int audition_percentage;
    public int status;
    public int start_date;
    public int release_date;
    public ActivityOwnerModel Users;
    public List<StoryVersion> StoryVersions;
    public List<ProjectCast> Project_cast;
    public List<Audition> Audition;
    public List<SceneModel> StoryScenes;
    public List<ProjectTeamModel> MyProjectTeam;
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
    public List<int> scene_characters;
    public DateTime updatedAt;
}

[Serializable]
public class ProjectsResponse : BaseResponse
{
    public List<Project> data;
}

[Serializable]
public class ProjectOfferedModel
{
    public int id;
    public int project_cast_id;
    public int user_id;
    public int project_id;
    public string description;
    public int role;
    public int payment_recieved;
    public int status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public Project Projects;
    public ActivityOwnerModel Users;
    public ProjectCast Project_cast;
}

[Serializable]
public class ProjectOfferOwnerModel
{
    public string title;
    public ActivityOwnerModel Users;
}


[Serializable]
public class ProjectOfferedResponse : BaseResponse
{
    public List<ProjectOfferedModel> data;
}


[Serializable]
public class ProjectDetailResponse : BaseResponse
{
    public Project data;
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
}

[Serializable]
public class SceneCharacter
{
    public int id;
    public int scene_id;
    public int character_id;
    public string dailogue;
    public DateTime created_date_time;
    public string updatedAt;
    public UserData Users;
}

[Serializable]
public class SceneDetailsModel
{
    public int id;
    public string place_type;
    public string description;
    public string shoot_time;
    public int story_id;
    public int scene_order;
    public int story_version_id;
    public int project_id;
    public string location;
    public int start_time;
    public int status;
    public DateTime created_date_time;
    public string updatedAt;
    public List<SceneCharacter> SceneCharacters;
    public List<MultimediaModel> ScenesMultimedia;
}

[Serializable]
public class SceneResponse : BaseResponse
{
    public SceneDetailsModel data;
}

[Serializable]
public class ProjectBrowserResponse : BaseResponse
{
    public List<PortfolioModel> data;
}

[Serializable]
public class ProjectCharacterModel : StoryCharacterModel
{
    public object project_cast_id;
    public int project_id;
    public object estimated_working_days;
    public int project_status;
    public string status;
    public int story_version_id;
    public List<MultimediaModel> CharacterMultimedia;
}

[Serializable]
public class ProjectCastDetailsResponse : BaseResponse
{
    public List<ProjectCast> data;
}

[Serializable]
public class UpdatedProjectTeamModel : BaseResponse
{
    public ProjectTeamModel data;
}

[Serializable]
public class ProjectTeamModel : StoryTeamModel
{
    public int project_id;

    public TeamMembersItem ProjectTeamMembers;
}

[Serializable]
public class UserDetailResponse : BaseResponse
{
    public UserData data;
}

[Serializable]
public class SceneCharacterAlbumResponse : BaseResponse
{
    public List<SceneAlbumModel> data;
}

[Serializable]
public class SceneAlbumModel : ProjectOfferedModel
{
    public SceneAlbumModel data;

    public bool isSeeAllSelected = false;
}
