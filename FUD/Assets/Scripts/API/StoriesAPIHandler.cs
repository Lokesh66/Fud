using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;
using TMPro;


public partial class APIHandler
{
    public void GetAllStories(int pageNo, Action<bool, List<StoryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.CREATE_STORY;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(GetRequest(url, true, (bool status, string response) => {
            if (status)
            {
                StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);
                action?.Invoke(true, stories.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetAllUsers(int pageNo, string sourceFrom, Action<bool, List<UserSearchModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("source_from", sourceFrom);

        string url = APIConstants.GET_ALL_USERS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (bool status, string response) => {

            Debug.Log("All users Response = " + response);

            UserSearchResponse searchResponse = JsonUtility.FromJson<UserSearchResponse>(response);

            if (status)
            {
                action?.Invoke(true, searchResponse.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetAllPublicStories(int pageNo, Action<bool, List<StoryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_PUBLIC_STORIES;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            Debug.Log("API Response = " + response);

            if (status)
            {
                StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);
                action?.Invoke(true, stories.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetAllHistories(int pageNo, int storyId, Action<bool, List<StoryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_HISTORY_STORIES;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            Debug.Log("API Response = " + response);

            if (status)
            {
                StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);
                action?.Invoke(true, stories.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void CreateStory(string title, string subTitle, string description, string posterURL, int genreId, int accessValue, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("story_line", subTitle);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("genre_id", genreId);

        if (posterURL.IsNOTNullOrEmpty())
        {
            parameters.Add("title_poster", posterURL);
        }

        if (multimediaModels.Count > 0)
        {
            parameters.Add("pro_multi_media", multimediaModels);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_STORY, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetAlteredStories(int pageNo, Action<bool, List<StoryAlteredModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_STORIES;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("tab_ name", "altered");

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            StoriesAlteredResponse stories = JsonUtility.FromJson<StoriesAlteredResponse>(response);

            action(status, stories.data);
        }));
    }

    public void UpdateStory(int storyId, string title, string subTitle, string description, string titlePosterURL, int genreId, int accessValue, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", storyId);

        parameters.Add("title", title);

        parameters.Add("story_line", subTitle);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("genre_id", genreId);

        if (titlePosterURL.IsNOTNullOrEmpty())
        {
            parameters.Add("title_poster", titlePosterURL);
        }

        if (multimediaModels.Count > 0)
        {
            parameters.Add("pro_multi_media", multimediaModels);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_STORY, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryDetails(int storyId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryHistories(int pageNo, int storyId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_HISTORY_STORIES;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_HISTORY_STORIES, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetPublicStoryDetails(int storyId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PUBLIC_STORY_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void CreateStoryVersion(int storyId, string description, int roleId, int accessValue, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("genre_id", roleId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("pro_multi_media", multimediaModels);
        }

        /*List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);*/

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_STORY_VERSION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryVersion(int storyId, int versionId, string description, int roleId, int accessValue, List<Dictionary<string, object>> multimediaModels, List<int> deletedMedia, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        int mediaStatus = multimediaModels.Count > 0 ? 1 : 0;

        parameters.Add("story_id", storyId);

        parameters.Add("only_media", mediaStatus);

        parameters.Add("id", versionId);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("genre_id", roleId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("pro_multi_media", multimediaModels);
        }

        if (deletedMedia.Count > 0)
        {
            parameters.Add("remove_media", deletedMedia);
        }
        /*List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);*/

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_STORY_VERSION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryVersionDetails(int id, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("fetch_media", 1);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_VERSION_DETAILS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemoveStoryVersion(int id, int storyId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("only_media", 0);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_STORY_VERSION, true, parameters, (apiStatus, response) => {

            action(apiStatus, response);
        }));
    }

    public void UpdateCharacter(int characterId, int storyId, string title, int castId, string description, int performerId, string gender, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", characterId);

        parameters.Add("story_id", storyId);

        parameters.Add("title", title);

        parameters.Add("type", castId);

        parameters.Add("description", description);

        parameters.Add("suitable_performer", performerId);

        parameters.Add("gender", gender);

        parameters.Add("status", 3);


        gameManager.StartCoroutine(PutRequest(APIConstants.SAVE_STORY_CHARACTER, true, parameters, (apiStatus, response) => {

            action(apiStatus, response);
        }));
    }

    public void CreateCharacter(CreateCharacterModel model, List<Dictionary<string, object>> multimedia, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", model.id);

        parameters.Add("title", model.title);

        parameters.Add("type", model.type);

        parameters.Add("role_id", model.craftId);

        parameters.Add("role_category_id", model.roleCategeryId);

        parameters.Add("description", model.description);

        parameters.Add("suitable_performer", model.characterId);

        parameters.Add("gender", model.gender);

        if (multimedia.Count > 0)
        {
            parameters.Add("character_multi_media", multimedia);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.SAVE_STORY_CHARACTER, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemoveCharacter(int id, int storyId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.SAVE_STORY_CHARACTER, true, parameters, (apiStatus, response) => {

            action(apiStatus, response);
        }));
    }

    public void UpdateCharacterStatus(int id, int storyId, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("only_media", 0);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_STORY_VERSION, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void CreateStoryTeam(int story_id, string title, string description, string members, int accessValue, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("members", members);

        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_STORY_TEAM, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryTeam(int story_id, string title, int teamId, string description, string members, int accessValue, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("id", teamId);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        parameters.Add("members", members);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_STORY_TEAM, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemoveTeam(int id, int storyId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_STORY_TEAM, true, parameters, (apiStatus, response) => {

            action(apiStatus, response);
        }));
    }

    public void SearchTeamMember(string keyword, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("value", keyword);

        parameters.Add("searchKey", "name");

        gameManager.StartCoroutine(PostRequest(APIConstants.SEARCH_TEAM_MEMBER, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryPost(int story_id, int versionId, string title, int postedTo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", story_id);

        parameters.Add("story_version_id", versionId);

        parameters.Add("posted_to", postedTo);


        if (title.IsNOTNullOrEmpty())
        {
            parameters.Add("title", title);
        }

        //parameters.Add("comment", comment);

        gameManager.StartCoroutine(PostRequest(APIConstants.STORY_POST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetOtherUserInfo(int id, int storyId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("story_id", storyId);

        gameManager.StartCoroutine(PostRequest(APIConstants.SEARCH_STORY_CHARACTER, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryPostStatus(int postId, int postStatus, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("post_id", postId);

        parameters.Add("status", postStatus);

        gameManager.StartCoroutine(PutRequest(APIConstants.STORY_POST, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateStoryBrowseStatus(int storyId, int postStatus, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("story_id", storyId);

        parameters.Add("status", postStatus);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_BROWSE_STATUS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetStoryPosts(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_STORY_POSTS;

        url += "?page="+ pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("tab_name", "offers");

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ApplyStoryOfferedFilter(int roleId, int sortId, int orderBy, Action<bool, List<StoryActivityModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

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

        string url = APIConstants.GET_STORY_POSTS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT +"&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (apiStatus, response) =>
        {
            StoryActivityResponseModel responseModel = JsonUtility.FromJson<StoryActivityResponseModel>(response);

            action(apiStatus, responseModel.data);
        }));
    }

    public void ApplyStoryAlteredFilter(int statusId, int sortId, int orderId, Action<bool, List<StoryAlteredModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

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

        string url = APIConstants.GET_ALTERED_STORIES;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (apiStatus, response) =>
        {
            StoriesAlteredResponse stories = JsonUtility.FromJson<StoriesAlteredResponse>(response);

            action(apiStatus, stories.data);
        }));
    }

    public void ApplyStoryBrowseFilter(int genreId, int storyTypeId, int sortId, int orderId, Action<bool, List<StoryModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        if (!sortId.Equals(-1))
        {
            parameters.Add("sortBy", sortId);
        }
        if (!genreId.Equals(-1))
        {
            parameters.Add("genre_id", genreId);
        }
        if (!orderId.Equals(-1))
        {
            parameters.Add("sortOrder", orderId);
        }
        if (!storyTypeId.Equals(-1))
        {
            parameters.Add("story_type", storyTypeId);
        }

        string url = APIConstants.GET_PUBLIC_STORIES;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (apiStatus, response) =>
        {
            StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);

            action(apiStatus, stories.data);
        }));
    }

    public void ApplyShareStoryFilter(string userName, int roleId, int roleCategeryId, int isCeleb, int industryId, Action<bool, List<UserSearchModel>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        if (userName.IsNOTNullOrEmpty())
        {
            parameters.Add("name", userName);
        }
        if (!roleId.Equals(-1))
        {
            parameters.Add("role_id", roleId);
        }
        if (!roleCategeryId.Equals(-1))
        {
            parameters.Add("role_category_id", roleCategeryId);
        }
        if (!isCeleb.Equals(-1))
        {
            parameters.Add("type", isCeleb);
        }
        if (!industryId.Equals(-1))
        {
            parameters.Add("industry_id", industryId);
        }

        string url = APIConstants.GET_ALL_USERS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (apiStatus, response) =>
        {
            UserSearchResponse stories = JsonUtility.FromJson<UserSearchResponse>(response);

            action(apiStatus, stories.data);
        }));
    }

    public void GetRoleCategeries(int roleId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("role_id", roleId);


        gameManager.StartCoroutine(PostRequest(APIConstants.GET_ROLE_CATEGERIES, true, parameters, (apiStatus, response) =>
        {
            action(apiStatus, response);
        }));
    }

    public void GetStoryTypes(Action<bool, string> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_STORY_TYPES, true, (apiStatus, response) =>
        {
            action(apiStatus, response);
        }));
    }

    public void UploadFile(string filePath, EMediaType mediaType, string mediaSource, Action<bool, string> OnResposne, string faceId = "")
    {
        gameManager.StartCoroutine(Upload(filePath, mediaType, mediaSource, faceId, (status, response) => {

            OnResposne?.Invoke(status, response);
        }));
    }
}

[Serializable]
public class StoryModel
{
    public int id;
    public string title;
    public string story_line;
    public string description;
    public int type_id;
    public int genre_id;
    public string title_poster;
    public int access_modifier;
    public int is_featured;
    public string status;
    public object created_date_time;
    public int user_id;
    public object created_by;
    public DateTime updatedAt;
}

[Serializable]
public class StoriesResponse : BaseResponse
{
    public List<StoryModel> data;
}

[Serializable]
public class StoryVersion
{
    public int id;
    public int story_id;
    public string description;
    public object ratings;
    public object price;
    public int genre_id;
    public int access_modifier;
    public object image_url;
    public object background_url;
    public object thumb_image_url;
    public object produced_by;
    public object developed_by;
    public int status;
    public object report;
    public object warning;
    public object privacy_policy;
    public DateTime created_date_time;
    public DateTime updated_date_time;

    public List<MultimediaModel> Multimedia;
}

[Serializable]
public class StoryDetailsModel
{
    public int id;
    public string title;
    public string story_line;
    public string description;
    public int type_id;
    public int genre_id;
    public int is_featured;
    public string status;
    public object created_date_time;
    public int user_id;
    public object created_by;
    public DateTime updatedAt;
    public List<StoryVersion> StoryVersions;
    public List<StoryCharacterModel> StoryCharacters;
    public List<StoryTeamModel> Myteam;
    public List<StoryHistoryModel> StoryTrack;
    public StoryDetailsController.EScreenSubType currentTab = 0;

    public DetailsScreenModel screenModel;

    public void SetDetailsScreenModel()
    {
        screenModel = new DetailsScreenModel(this);
    }
}

[Serializable]
public class StoryCharacterModel
{
    public int id;
    public int story_id;
    public int type;
    public int role_id;
    public int role_category_id;
    public string title;
    public string description;
    public string suitable_performer;
    public string gender;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public CraftRoleModel Craftroles;
    public RoleCategoryModel RoleCategories;
    public ActivityOwnerModel UserInfo;

    public string GetCharacterType(int castId)
    {
        return castId > 0 ? "crew" : "cast";
    }
}

[Serializable]
public class CraftRoleModel
{
    public int id;
    public string name;
}

[Serializable]
public class RoleCategoryModel
{
    public int id;
    public string name;
}

[Serializable]
public class StoryTeamModel
{
    public int id;
    public int story_id;
    public int user_id;
    public string description;
    public string members;
    public int status;
    public DateTime created_date_time;
    public string title;
    public DateTime updatedAt;
    public List<TeamMembersItem> TeamMembers;
}

[Serializable]
public class StoryHistoryModel
{
    public int id;
    public int story_id;
    public int from_user;
    public object to_user;
    public int creator_id;
    public string activity;
    public object created_date_time;
    public int updated_date_time;
    public ActivityOwnerModel fromUser;
}

[Serializable]
public class TeamMembersItem
{
    public int id;
    public int team_id;
    public int user_id;
    public int status;
    public string created_date_time;
    public string updatedAt;
    public UserData users;
}

[Serializable]
public class DetailsScreenModel
{
    public string description;
    public int genreId;

    public DetailsScreenModel(StoryDetailsModel detailsModel)
    {
        description = detailsModel.description;

        genreId = detailsModel.genre_id;
    }
}

[Serializable]
public class StoryDetailsResponseModel : BaseResponse
{
    public List<StoryDetailsModel> data;
}

[Serializable]
public class MultimediaModel
{
    public int id = -1;
    public int story_version_id;
    public int work_exp_id;
    public int port_album_id;
    public int source_id;
    public string source_type;
    public string media_type;
    public string content_url;
    public string status;
    public int content_id;
    public int related_content_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;


    public EMediaType GetMediaType(string _mediaType)
    {
        EMediaType mediaType = EMediaType.Image;

        switch (_mediaType)
        {
            case "image":
                mediaType = EMediaType.Image;
                break;
            case "audio":
                mediaType = EMediaType.Audio;
                break;
            case "video":
                mediaType = EMediaType.Video;
                break;
        }

        return mediaType;
    }
}

[Serializable]
public class StoryVersionDetailModel : StoryVersion
{

}

[Serializable]
public class MultiMediaResponse : BaseResponse
{
    public List<StoryVersionDetailModel> data;
}

[Serializable]
public class UserSearchModel : ActivityOwnerModel
{
    public string email_id;
    public List<WorkExperianceModel> WorkExperience;
}

[Serializable]
public class UserSearchResponse : BaseResponse
{
    public List<UserSearchModel> data;
}

[Serializable]
public class UpdatedCharaterModel : BaseResponse
{
    public StoryCharacterModel data;
}

[Serializable]
public class UpdatedStoryResponse : BaseResponse
{
    public StoryModel data;
}

[Serializable]
public class UpdatedTeamModel : BaseResponse
{
    public StoryTeamModel data;
}


[Serializable]
public class StoryActivityModel
{
    public int id;
    public string title;
    public int posted_to;
    public int story_id;
    public int source_id;
    public string source_type;
    public int user_id;
    public int sender_status;
    public int reciever_status;
    public string comment;
    public DateTime created_date_time;
    public DateTime updatedAt;

    public StoryVersion StoryVersions;
}


[Serializable]
public class StoryActivityResponseModel : BaseResponse
{
    public List<StoryActivityModel> data;
}

[Serializable]
public class StoryVersions
{
    public int id;
    public int story_id;
    public string description;
    public object ratings;
    public object price;
    public int genre_id;
    public string image_url;
    public string background_url;
    public string thumb_image_url;
    public string produced_by;
    public string developed_by;
    public int status;
    public string report;
    public string warning;
    public string privacy_policy;
    public int project_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;

    public List<MultimediaModel> Multimedia;
}

[Serializable]
public class ProjectStory
{
    public int id;
    public string title;
    public int posted_to;
    public int story_id;
    public int story_version_id;
    public int source_id;
    public string source_type;
    public int user_id;
    public string comment;
    public int sender_status;
    public int reciever_status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public StoryModel Stories;
} 

[Serializable]
public class ProjectStoriesResponse : BaseResponse
{
    public List<ProjectStory> data;
}


[Serializable]
public class CreatedStoryVerionModel : StoryVersion
{
   
}

[Serializable]
public class UpdatedStoryVersionResponse : BaseResponse
{
    public List<StoryVersion> data;
}

[Serializable]
public class CreatedStoryVersionResponse : BaseResponse
{
    public CreatedStoryVerionModel data;
}

[Serializable]
public class PerformerModel
{
    public int id;
    public int story_id;
    public string title;
    public string description;
    public string suitable_performer;
    public object project_cast_id;
    public object project_id;
    public object estimated_working_days;
    public object project_status;
    public string status;
    public object story_version_id;
    public string gender;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public UserData UserInfo;
}

[Serializable]
public class PerformerResponse : BaseResponse
{
    public PerformerModel data;
}

[Serializable]
public class ProfileFileUploadModel
{
    public string Location;
    public string Key;
    public string Bucket;
    public string ETag;
    public string faceId;
    public bool isCeleb;
}

[Serializable]
public class ProfileUploadResponseModel : BaseResponse
{
    public ProfileFileUploadModel data;
}

[Serializable]
public class FileUploadResponseModel : BaseResponse
{
    public string data;
    public long timestamp;
}

[Serializable]
public class StoryAlteredModel
{
    public int id;
    public string title;
    public int posted_to; 
    public int story_id;
    public int story_version_id;
    public int source_id;
    public string source_type;
    public int user_id;
    public object comment;
    public int sender_status;
    public int reciever_status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public StoryVersion StoryVersions;
    public ActivityOwnerModel Users;
    public Stories Stories;
}

[Serializable]
public class Stories
{
    public string title_poster;
}

[Serializable]
public class StoriesAlteredResponse : BaseResponse
{
    public List<StoryAlteredModel> data;
}


[Serializable]
public class RoleCategeryModel
{
    public int id;
    public int role_id;
    public string name;
    public string status;
    public DateTime created_date;
    public object updatedAt;
}

[Serializable]
public class RoleCategeryResponse : BaseResponse 
{
    public List<RoleCategeryModel> data;
}

[Serializable]
public class StoryBrowseDetailResponse : BaseResponse
{
    public StoryActivityModel data;
}

[Serializable]
public class HistoryResponses
{
    public List<StoryHistoryModel> StoryTrack;
}

[Serializable]
public class StoryHistoryResponse : BaseResponse
{
    public List<HistoryResponses> data;
}

[Serializable]
public class StoryTypesResponse
{
    public List<StoryTypeModel> data;
}

[Serializable]
public class StoryTypeModel
{
    public int id;
    public string name;
}

public class CreateCharacterModel
{
    public int id;
    public string title;
    public int type;
    public string description;
    public string gender;
    public int characterId;
    public int craftId;
    public int roleCategeryId;
}

